using System;
using System.Timers;

namespace FrizzLib.CLI;

/// <summary>
/// A static class containing useful CLI methods.
/// <para><u>Available methods</u></para>
/// <list type="bullet">
/// <item><see cref="Prompt(string)"/></item>
/// <item><see cref="Prompt(string, string)"/></item>
/// <item><see cref="Prompt(string, string, bool)"/></item>
/// <item><see cref="WriteHeading(string)"/></item>
/// <item><see cref="ProgressBar_Start(int)"/></item>
/// <item><see cref="ProgressBar_Stop"/></item>
/// </list>
/// </summary>
public static class CLI
{
    #region Prompt
    /// <summary>
    /// Displays a message, and on the next line prompts user to press ENTER to continue.
    /// </summary>
    /// <param name="PromptText">
    /// <ulist type="bullet">
    /// <li>The message to display.</li>
    /// </ulist>
    /// </param>
    public static void Prompt(string PromptText)
    {
        Prompt(PromptText, "continue");
    }


    /// <summary>
    /// Displays a message, and on the next line prompts user to press ENTER to perform another action.
    /// </summary>
    /// <param name="PromptText">The message to display.</param>
    /// <param name="ContinueText">The action to perform after user presses ENTER.</param>
    public static void Prompt(string PromptText, string ContinueText)
    {
        Prompt(PromptText, ContinueText, false);
    }

    /// <summary>
    /// Displays a message, and on the next or the same line prompts user to press ENTER to perform another action.
    /// </summary>
    /// <param name="PromptText">The message to display.</param>
    /// <param name="ContinueText">The action to perform after user presses ENTER.</param>
    /// <param name="OneLinePrompt">bool specifying whether to display both messages on a single line or not.</param>
    public static void Prompt(string PromptText, string ContinueText, bool OneLinePrompt)
    {
        PromptText += OneLinePrompt ? " " : "\n";
        Console.Write($"{PromptText}Press ENTER to {ContinueText}: ");
        Console.ReadLine();
    }
    #endregion

    #region WriteHeading
    /// <summary>
    /// Write a string to the console with a line of underscores above it to highlight and separate the string from prior console output.
    /// </summary>
    /// <param name="Heading">The text to output to the console.</param>
    public static void WriteHeading(string Heading)
    {
        string line = new('_', Heading.Length);
        Console.WriteLine($"{line}\n{Heading}");
    }
    #endregion

    #region ProgressBar
    static System.Timers.Timer? timer;
    /// <summary>
    /// Starts a CLI 'Progress bar' that outputs a period to the console at regular intervals.
    /// </summary>
    /// <param name="msInterval">The interval (in milliseconds).</param>
    public static void ProgressBar_Start(int msInterval)
    {
        if (timer != null) timer.Stop();
        else
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += IntervalAction;
        }
        timer.Interval = msInterval;
        timer.Start();
    }
    /// <summary>
    /// Stops the Progress bar.
    /// </summary>
    public static void ProgressBar_Stop()
    {
        if (timer == null) return;
        timer.Stop();
        timer.Close();
    }
    // Timer callback method
    private static void IntervalAction(object? sender, ElapsedEventArgs e)
    {
        Console.Write(".");
    }
    #endregion
}
