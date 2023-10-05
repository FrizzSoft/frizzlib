namespace FrizzLib.CLI.Pickers;

/// <summary>
/// <para>Abstract class which allows the listing of an array of items of type T. Classes that inherit
/// from this class specify the type (T) of items to be listed, and the implementation details of the listing.
/// Each item listed is given a number so that the user can select one of the items by its number.
/// User can elect to display the items in batches of up to 100 items, and
/// can type <em>?</em> to get Help or <em>ENTER</em> to continue after each batch. User-defined messages can
/// be displayed before and after each batch of items listed. Virtual methods are provided which can be
/// over-ridden by the user to determine how each item is displayed in the list, and allow additional
/// valid text responses.</para>
/// <p><see cref="PickItem(T[], int)"></see> pauses after each batch and accepts user input. The class abstracts away the recursion logic of the listing process.</p>
/// <p>The following methods may be overridden:</p>
/// <list type="bullet">
/// <item><see cref="ItemFormatter(T)"></see> Logic to appropriately display one object to the console</item>
/// <item><see cref="IsAUserAllowedResponse(string?)"></see> Allows user to include additional allowed text responses</item>
/// </list>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ItemPicker<T>
{
    private int batchSize;
    private int arrayOffset;
    private static readonly string prompt = "\n\nEnter your response (? for help): ";
    /// <summary>
    /// The text to display above each batch of items listed.
    /// </summary>
    public string BatchHeading { get; set; } = "";
    /// <summary>
    /// The Help text to display when the user types ? to request help.
    /// </summary>
    public string HelpMessage { get; set; } = "Select item by number, or press ENTER to continue if listing paused." + prompt;
    /// <summary>
    /// The text to display after a batch of items has been listed, and more items are to follow.
    /// </summary>
    public string Prompt_AfterBatch { get; set; } = "more (press ENTER)..." + prompt;
    /// <summary>
    /// The text to display after the final batch of items has been listed, and there are no more items to display.
    /// </summary>
    public string Prompt_AfterFinalBatch { get; set; } = "END OF LISTING." + prompt;

    #region PickItem (lists items in batches recursively, getting user response after each batch)
    /// <summary>
    /// Lists items in batches recursively, getting user response after each batch.
    /// </summary>
    /// <param name="Items">The array of items to list and choose from.</param>
    /// <param name="BatchSize">How many items to display to the console at a time.</param>
    /// <returns>
    /// <p>Returns a string, or null. Cannot return an empty string, as pressing ENTER by itself is reserved
    /// to indicate that the next batch of items should be displayed. If the string returned contains digits
    /// only, then this will represent the array index of the selected item.  The method will not allow entry
    /// of numbers outside the bounds of the array.</p>
    /// 
    /// <p>Possible user responses:</p>
    /// <list type="bullet">
    /// <item>
    ///     <term>ENTER on its own</term>
    ///     <description>triggers listing of the next batch of items (if there are no more items, the method returns null)</description>
    /// </item>    
    /// <item>
    ///     <term>a string of digits representing the index of one of the listed items</term>
    ///     <description>
    ///     If the integer representation of the string of digits is outside the range
    ///     of the items listed, then the method re-prompts the user for a valid response.
    ///     </description>
    /// </item> 
    /// <item>
    ///     <term>any other text which is not a string of digits only</term>
    ///     <description>if the text is allowed by <see cref="IsAUserAllowedResponse">IsAUserAllowedResponse()</see>,
    ///     terminates the listing process and returns the user-entered text</description>
    /// </item>
    /// <item>
    ///     <term>CTRL-Z ENTER</term>
    ///     <description>terminates the listing and returns null</description>
    /// </item> 
    /// </list>
    /// </returns>
    /// <remarks>
    /// A returned value of null should be taken to indicate that the user has not chosen an item from the list.
    /// </remarks>
    public string? PickItem(T[] Items, int BatchSize)
    {
        if (BatchSize > 100) throw new ArgumentException("BatchSize cannot be greater than 100.");
        string? response;
        batchSize = BatchSize;
        arrayOffset = 0;
        response = PickItem(Items);
        return IfNumeric_AddOffset(response, arrayOffset);   // Return user's response (if numeric, it is the index of selected item)

        // Local function
        static string? IfNumeric_AddOffset(string? response, int arrayOffset)
        {
            if (!int.TryParse(response, out int number)) return response;
            else return (number + arrayOffset).ToString();
        }
    }

    private string? PickItem(T[] Items)
    {
        string? response;
        int remainingItemsCount = Items.Length;
        if (remainingItemsCount > batchSize)
        {
            ListOneBatch(Items, batchSize);                             // List one batch
            Console.Write(Prompt_AfterBatch);
            response = GetValidResponse(batchSize, IsFinalBatch: false);
            while (response == "")                                      // List next batch if user presses ENTER
            {
                arrayOffset += batchSize;
                response = PickItem(Items[batchSize..]);
            }
        }
        else
        {
            ListOneBatch(Items, remainingItemsCount);                   // List final items
            Console.Write("\n" + Prompt_AfterFinalBatch);
            response = GetValidResponse(remainingItemsCount, IsFinalBatch: true);
        }
        return response;

        // Helper method
        void ListOneBatch(T[] Items, int NumberToList)
        {
            int currentItemIndex = 0;
            string tempString;
            string nextOutputLine = "";
            bool twoColumnsWillWork;
            bool lastItem;
            Display_BatchHeading();

            if (Items.Length == 0)
            {
                return;
            }
            twoColumnsWillWork = GetMaxLengthOfItems(Items) < 46;
            if (AnyItemSpansMoreThanOneLine(Items)) twoColumnsWillWork = false;
            do
            {
                string itemText = $"{currentItemIndex:00} " + ItemFormatter(Items[currentItemIndex]);
                lastItem = currentItemIndex == NumberToList - 1;
                if (twoColumnsWillWork)
                {
                    tempString = String.Format("{0, -50}", itemText);
                    nextOutputLine = currentItemIndex % 2 == 0 ? tempString : nextOutputLine + tempString;
                    if (currentItemIndex % 2 == 1 || lastItem)
                    {
                        Console.WriteLine(nextOutputLine);
                        nextOutputLine = "";
                        if (lastItem) break;
                    }
                }
                else
                {
                    Console.WriteLine(itemText);
                }
            } while (++currentItemIndex < NumberToList);

            // Helper methods
            int GetMaxLengthOfItems(T[] items)
            {
                int maxLength = 0;
                foreach (T item in items)
                {
                    maxLength = Math.Max(maxLength, ItemFormatter(item).Length);
                }
                return maxLength;
            }

            bool AnyItemSpansMoreThanOneLine(T[] items)
            {
                foreach (T item in items)
                {
                    if (ItemFormatter(item).Contains('\n')) return true;
                }
                return false;
            }
        }
    }

    private void Display_BatchHeading()
    {
        var savedForegroundColor = Console.ForegroundColor;
        var savedBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.Write("\n" + BatchHeading);
        Console.ForegroundColor = savedForegroundColor;
        Console.BackgroundColor = savedBackgroundColor;
        Console.WriteLine();
    }
    #endregion

    // Checks for appropriate use of ENTER (to continue listing), ? (for help), and numeric item selection
    private string? GetValidResponse(int NumItemsInBatch, bool IsFinalBatch)
    {
        string? response;
        response = Console.ReadLine();
        if (response == "?")                 // Display Help message
        {
            Console.Write(HelpMessage);
            return GetValidResponse(NumItemsInBatch, IsFinalBatch);
        }
        if (response == "")
        {
            if (IsFinalBatch)   // Cannot opt for another batch if no more batches
            {
                Console.Write("No more items to list! Please choose again: ");
                return GetValidResponse(NumItemsInBatch, IsFinalBatch);
            }
            else return response;
        }
        if (int.TryParse(response, out int number))
        {
            // If response is numeric, check that it is an index of one of the listed items
            if (number >= 0 && number < NumItemsInBatch)
                return response;                // In range
                                                // Numeric response is out of range, so re-prompt
            System.Console.Write("That is not an item listed. Please choose again: ");
            return GetValidResponse(NumItemsInBatch, IsFinalBatch);
        }
        if (IsAUserAllowedResponse(response)) return response;
        Console.Write("Invalid response. Please try again: ");
        return GetValidResponse(NumItemsInBatch, IsFinalBatch);
    }

    #region Virtual methods: Override these to enable listing of different objects and validate user responses
    /// <summary>
    /// Override this method to provide logic to display each item T.
    /// </summary>
    /// <param name="t">The object to display.</param>
    /// <returns>The string representing how the item should be displayed to the console. By default the method simply returns
    /// t.ToString(), or an empty string if t is null.</returns>
    protected virtual string ItemFormatter(T t)
    {
        return t?.ToString() ?? "";
    }
    /// <summary>
    /// Checks a string to determine if it is a valid response. By default all strings are valid.
    /// </summary>
    /// <param name="Response"></param>
    /// <returns>true if <c>Response</c> is allowed, else false</returns>
    protected virtual bool IsAUserAllowedResponse(string? Response)
    {
        // No validation by default.  Override to add user validation logic.
        // Returns true if the string? Response is allowed, else false
        return true;
    }
    #endregion
}