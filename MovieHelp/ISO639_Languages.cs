using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Linq;

namespace FrizzLib.MovieHelp;

public static class ISO639_Language
{
    // Private fields
    private static readonly List<LanguageModel> languages = new();

    // Static constructor
    static ISO639_Language()
    {
        languages = InitializeLanguages();
    }

    #region Public methods
    public static string? GetValidISO639_2Code(string? LanguageString)
    // Returns a 3 character ISO639-2 language string if LanguageString matches a known language
    // Returns null if no language match found
    {
        if (LanguageString == null) return null;
        int StringLength = LanguageString.Length;
        if (StringLength < 2) return null;
        if (StringLength == 2) return GetISO639_2_FromISO639_1(LanguageString);
        if (StringLength == 3) return FindISO639_2(LanguageString);
        return GetISO639_2_FromLongLanguage(LanguageString);
    }

    public static string? LongLanguage_FromISO639_2(string? LanguageString)
    {
        if (LanguageString == null) return null;
        return (from lang in languages
                where lang.ISO639_2 == LanguageString
                select lang).FirstOrDefault()?.LanguageLong;
    }
    #endregion Public methods

    #region Private members
    static string? GetISO639_2_FromLongLanguage(string LongLang)
    // Return 3 character ISO639-2 language code if LongLang match found
    // Returns null if LongLang not found in languages list
    {
        return (from lang in languages
                where lang.LanguageLong.ToLower() == LongLang.ToLower()
                select lang).FirstOrDefault()?.ISO639_2;
    }

    static string? FindISO639_2(string Lang3Char)
    // Return 3 character ISO639-2 language code if Lang3Char match found
    // Returns null if Lang3Char not found in languages ISO639-2 list
    {
        return (from lang in languages
                where lang.ISO639_2 == Lang3Char.ToLower()
                select lang).FirstOrDefault()?.ISO639_2;
    }

    static string? GetISO639_2_FromISO639_1(string Lang2Char)
    // Return 3 character ISO639-2 language code if Lang2Char match found
    // Returns null if Lang2Char not found in languages ISO639-1 list
    {
        return (from lang in languages
                where lang.ISO639_1 == Lang2Char.ToLower()
                select lang).FirstOrDefault()?.ISO639_2;
    }

    private static List<LanguageModel> InitializeLanguages()
    {
        languages.Add(new LanguageModel("ab", "abk", "abk", "Abkhazian"));
        languages.Add(new LanguageModel("aa", "aar", "aar", "Afar"));
        languages.Add(new LanguageModel("af", "afr", "afr", "Afrikaans"));
        languages.Add(new LanguageModel("ak", "aka", "aka", "Akan"));
        languages.Add(new LanguageModel("sq", "alb", "alb", "Albanian"));
        languages.Add(new LanguageModel("am", "amh", "amh", "Amharic"));
        languages.Add(new LanguageModel("ar", "ara", "ara", "Arabic"));
        languages.Add(new LanguageModel("an", "arg", "arg", "Aragonese"));
        languages.Add(new LanguageModel("hy", "arm", "arm", "Armenian"));
        languages.Add(new LanguageModel("as", "asm", "asm", "Assamese"));
        languages.Add(new LanguageModel("av", "ava", "ava", "Avaric"));
        languages.Add(new LanguageModel("ae", "ave", "ave", "Avestan"));
        languages.Add(new LanguageModel("ay", "aym", "aym", "Aymara"));
        languages.Add(new LanguageModel("az", "aze", "aze", "Azerbaijani"));
        languages.Add(new LanguageModel("bm", "bam", "bam", "Bambara"));
        languages.Add(new LanguageModel("ba", "bak", "bak", "Bashkir"));
        languages.Add(new LanguageModel("eu", "baq", "baq", "Basque"));
        languages.Add(new LanguageModel("be", "bel", "bel", "Belarusian"));
        languages.Add(new LanguageModel("bn", "ben", "ben", "Bengali"));
        languages.Add(new LanguageModel("bh", "bih", "bih", "Bihari languages"));
        languages.Add(new LanguageModel("bi", "bis", "bis", "Bislama"));
        languages.Add(new LanguageModel("bs", "bos", "bos", "Bosnian"));
        languages.Add(new LanguageModel("br", "bre", "bre", "Breton"));
        languages.Add(new LanguageModel("bg", "bul", "bul", "Bulgarian"));
        languages.Add(new LanguageModel("my", "bur", "bur", "Burmese"));
        languages.Add(new LanguageModel("ca", "cat", "cat", "Catalan"));
        languages.Add(new LanguageModel("ch", "cha", "cha", "Chamorro"));
        languages.Add(new LanguageModel("ce", "che", "che", "Chechen"));
        languages.Add(new LanguageModel("zh", "chi", "chi", "Chinese"));
        languages.Add(new LanguageModel("cu", "chu", "chu", "Church Slavic"));
        languages.Add(new LanguageModel("cv", "chv", "chv", "Chuvash"));
        languages.Add(new LanguageModel("kw", "cor", "cor", "Cornish"));
        languages.Add(new LanguageModel("co", "cos", "cos", "Corsican"));
        languages.Add(new LanguageModel("cr", "cre", "cre", "Cree"));
        languages.Add(new LanguageModel("hr", "hrv", "hrv", "Croatian"));
        languages.Add(new LanguageModel("cs", "cze", "cze", "Czech"));
        languages.Add(new LanguageModel("da", "dan", "dan", "Danish"));
        languages.Add(new LanguageModel("dv", "div", "div", "Dhivehi"));
        languages.Add(new LanguageModel("nl", "dut", "dut", "Dutch"));
        languages.Add(new LanguageModel("dz", "dzo", "dzo", "Dzongkha"));
        languages.Add(new LanguageModel("en", "eng", "eng", "English"));
        languages.Add(new LanguageModel("eo", "epo", "epo", "Esperanto"));
        languages.Add(new LanguageModel("et", "est", "est", "Estonian"));
        languages.Add(new LanguageModel("ee", "ewe", "ewe", "Ewe"));
        languages.Add(new LanguageModel("fo", "fao", "fao", "Faroese"));
        languages.Add(new LanguageModel("fj", "fij", "fij", "Fijian"));
        languages.Add(new LanguageModel("fi", "fin", "fin", "Finnish"));
        languages.Add(new LanguageModel("fr", "fre", "fre", "French"));
        languages.Add(new LanguageModel("ff", "ful", "ful", "Fulah"));
        languages.Add(new LanguageModel("gl", "glg", "glg", "Galician"));
        languages.Add(new LanguageModel("lg", "lug", "lug", "Ganda"));
        languages.Add(new LanguageModel("ka", "geo", "geo", "Georgian"));
        languages.Add(new LanguageModel("de", "ger", "ger", "German"));
        languages.Add(new LanguageModel("el", "gre", "gre", "Greek"));
        languages.Add(new LanguageModel("gn", "grn", "grn", "Guarani"));
        languages.Add(new LanguageModel("gu", "guj", "guj", "Gujarati"));
        languages.Add(new LanguageModel("ht", "hat", "hat", "Haitian"));
        languages.Add(new LanguageModel("ha", "hau", "hau", "Hausa"));
        languages.Add(new LanguageModel("he", "heb", "heb", "Hebrew"));
        languages.Add(new LanguageModel("hz", "her", "her", "Herero"));
        languages.Add(new LanguageModel("hi", "hin", "hin", "Hindi"));
        languages.Add(new LanguageModel("ho", "hmo", "hmo", "Hiri Motu"));
        languages.Add(new LanguageModel("hu", "hun", "hun", "Hungarian"));
        languages.Add(new LanguageModel("is", "ice", "ice", "Icelandic"));
        languages.Add(new LanguageModel("io", "ido", "ido", "Ido"));
        languages.Add(new LanguageModel("ig", "ibo", "ibo", "Igbo"));
        languages.Add(new LanguageModel("id", "ind", "ind", "Indonesian"));
        languages.Add(new LanguageModel("ia", "ina", "ina", "Interlingua"));
        languages.Add(new LanguageModel("ie", "ile", "ile", "Interlingue"));
        languages.Add(new LanguageModel("iu", "iku", "iku", "Inuktitut"));
        languages.Add(new LanguageModel("ik", "ipk", "ipk", "Inupiaq"));
        languages.Add(new LanguageModel("ga", "gle", "gle", "Irish"));
        languages.Add(new LanguageModel("it", "ita", "ita", "Italian"));
        languages.Add(new LanguageModel("ja", "jpn", "jpn", "Japanese"));
        languages.Add(new LanguageModel("jv", "jav", "jav", "Javanese"));
        languages.Add(new LanguageModel("kl", "kal", "kal", "Kalaallisut"));
        languages.Add(new LanguageModel("kn", "kan", "kan", "Kannada"));
        languages.Add(new LanguageModel("kr", "kau", "kau", "Kanuri"));
        languages.Add(new LanguageModel("ks", "kas", "kas", "Kashmiri"));
        languages.Add(new LanguageModel("kk", "kaz", "kaz", "Kazakh"));
        languages.Add(new LanguageModel("km", "khm", "khm", "Khmer"));
        languages.Add(new LanguageModel("ki", "kik", "kik", "Kikuyu"));
        languages.Add(new LanguageModel("rw", "kin", "kin", "Kinyarwanda"));
        languages.Add(new LanguageModel("ky", "kir", "kir", "Kirghiz"));
        languages.Add(new LanguageModel("kv", "kom", "kom", "Komi"));
        languages.Add(new LanguageModel("kg", "kon", "kon", "Kongo"));
        languages.Add(new LanguageModel("ko", "kor", "kor", "Korean"));
        languages.Add(new LanguageModel("kj", "kua", "kua", "Kuanyama"));
        languages.Add(new LanguageModel("ku", "kur", "kur", "Kurdish"));
        languages.Add(new LanguageModel("lo", "lao", "lao", "Lao"));
        languages.Add(new LanguageModel("la", "lat", "lat", "Latin"));
        languages.Add(new LanguageModel("lv", "lav", "lav", "Latvian"));
        languages.Add(new LanguageModel("li", "lim", "lim", "Limburgan"));
        languages.Add(new LanguageModel("ln", "lin", "lin", "Lingala"));
        languages.Add(new LanguageModel("lt", "lit", "lit", "Lithuanian"));
        languages.Add(new LanguageModel("lu", "lub", "lub", "Luba-Katanga"));
        languages.Add(new LanguageModel("lb", "ltz", "ltz", "Luxembourgish"));
        languages.Add(new LanguageModel("mk", "mac", "mac", "Macedonian"));
        languages.Add(new LanguageModel("mg", "mlg", "mlg", "Malagasy"));
        languages.Add(new LanguageModel("ms", "may", "may", "Malay"));
        languages.Add(new LanguageModel("ml", "mal", "mal", "Malayalam"));
        languages.Add(new LanguageModel("mt", "mlt", "mlt", "Maltese"));
        languages.Add(new LanguageModel("gv", "glv", "glv", "Manx"));
        languages.Add(new LanguageModel("mi", "mao", "mao", "Maori"));
        languages.Add(new LanguageModel("mr", "mar", "mar", "Marathi"));
        languages.Add(new LanguageModel("mh", "mah", "mah", "Marshallese"));
        languages.Add(new LanguageModel("mn", "mon", "mon", "Mongolian"));
        languages.Add(new LanguageModel("na", "nau", "nau", "Nauru"));
        languages.Add(new LanguageModel("nv", "nav", "nav", "Navajo"));
        languages.Add(new LanguageModel("ng", "ndo", "ndo", "Ndonga"));
        languages.Add(new LanguageModel("ne", "nep", "nep", "Nepali"));
        languages.Add(new LanguageModel("nd", "nde", "nde", "North Ndebele"));
        languages.Add(new LanguageModel("se", "sme", "sme", "Northern Sami"));
        languages.Add(new LanguageModel("nb", "nob", "nob", "Norwegian Bokmål"));
        languages.Add(new LanguageModel("nn", "nno", "nno", "Norwegian Nynorsk"));
        languages.Add(new LanguageModel("no", "nor", "nor", "Norwegian"));
        languages.Add(new LanguageModel("ny", "nya", "nya", "Nyanja"));
        languages.Add(new LanguageModel("oc", "oci", "oci", "Occitan"));
        languages.Add(new LanguageModel("oj", "oji", "oji", "Ojibwa"));
        languages.Add(new LanguageModel("or", "ori", "ori", "Oriya"));
        languages.Add(new LanguageModel("om", "orm", "orm", "Oromo"));
        languages.Add(new LanguageModel("os", "oss", "oss", "Ossetian"));
        languages.Add(new LanguageModel("pi", "pli", "pli", "Pali"));
        languages.Add(new LanguageModel("pa", "pan", "pan", "Panjabi"));
        languages.Add(new LanguageModel("fa", "per", "per", "Persian"));
        languages.Add(new LanguageModel("", "fil", "fil", "Filipino"));
        languages.Add(new LanguageModel("pl", "pol", "pol", "Polish"));
        languages.Add(new LanguageModel("pt", "por", "por", "Portuguese"));
        languages.Add(new LanguageModel("ps", "pus", "pus", "Pushto"));
        languages.Add(new LanguageModel("qu", "que", "que", "Quechua"));
        languages.Add(new LanguageModel("ro", "rum", "rum", "Romanian"));
        languages.Add(new LanguageModel("rm", "roh", "roh", "Romansh"));
        languages.Add(new LanguageModel("rn", "run", "run", "Rundi"));
        languages.Add(new LanguageModel("ru", "rus", "rus", "Russian"));
        languages.Add(new LanguageModel("sm", "smo", "smo", "Samoan"));
        languages.Add(new LanguageModel("sg", "sag", "sag", "Sango"));
        languages.Add(new LanguageModel("sa", "san", "san", "Sanskrit"));
        languages.Add(new LanguageModel("sc", "srd", "srd", "Sardinian"));
        languages.Add(new LanguageModel("gd", "gla", "gla", "Scottish Gaelic"));
        languages.Add(new LanguageModel("sr", "srp", "srp", "Serbian"));
        languages.Add(new LanguageModel("sn", "sna", "sna", "Shona"));
        languages.Add(new LanguageModel("ii", "iii", "iii", "Sichuan Yi"));
        languages.Add(new LanguageModel("sd", "snd", "snd", "Sindhi"));
        languages.Add(new LanguageModel("si", "sin", "sin", "Sinhala"));
        languages.Add(new LanguageModel("sk", "slo", "slo", "Slovak"));
        languages.Add(new LanguageModel("sl", "slv", "slv", "Slovenian"));
        languages.Add(new LanguageModel("so", "som", "som", "Somali"));
        languages.Add(new LanguageModel("nr", "nbl", "nbl", "South Ndebele"));
        languages.Add(new LanguageModel("st", "sot", "sot", "Southern Sotho"));
        languages.Add(new LanguageModel("es", "spa", "spa", "Spanish"));
        languages.Add(new LanguageModel("su", "sun", "sun", "Sundanese"));
        languages.Add(new LanguageModel("sw", "swa", "swa", "Swahili"));
        languages.Add(new LanguageModel("ss", "ssw", "ssw", "Swati"));
        languages.Add(new LanguageModel("sv", "swe", "swe", "Swedish"));
        languages.Add(new LanguageModel("tl", "tgl", "tgl", "Tagalog"));
        languages.Add(new LanguageModel("ty", "tah", "tah", "Tahitian"));
        languages.Add(new LanguageModel("tg", "tgk", "tgk", "Tajik"));
        languages.Add(new LanguageModel("ta", "tam", "tam", "Tamil"));
        languages.Add(new LanguageModel("tt", "tat", "tat", "Tatar"));
        languages.Add(new LanguageModel("te", "tel", "tel", "Telugu"));
        languages.Add(new LanguageModel("th", "tha", "tha", "Thai"));
        languages.Add(new LanguageModel("bo", "tib", "tib", "Tibetan"));
        languages.Add(new LanguageModel("ti", "tir", "tir", "Tigrinya"));
        languages.Add(new LanguageModel("to", "ton", "ton", "Tonga"));
        languages.Add(new LanguageModel("ts", "tso", "tso", "Tsonga"));
        languages.Add(new LanguageModel("tn", "tsn", "tsn", "Tswana"));
        languages.Add(new LanguageModel("tr", "tur", "tur", "Turkish"));
        languages.Add(new LanguageModel("tk", "tuk", "tuk", "Turkmen"));
        languages.Add(new LanguageModel("tw", "twi", "twi", "Twi"));
        languages.Add(new LanguageModel("ug", "uig", "uig", "Uighur"));
        languages.Add(new LanguageModel("uk", "ukr", "ukr", "Ukrainian"));
        languages.Add(new LanguageModel("ur", "urd", "urd", "Urdu"));
        languages.Add(new LanguageModel("uz", "uzb", "uzb", "Uzbek"));
        languages.Add(new LanguageModel("ve", "ven", "ven", "Venda"));
        languages.Add(new LanguageModel("vi", "vie", "vie", "Vietnamese"));
        languages.Add(new LanguageModel("vo", "vol", "vol", "Volapük"));
        languages.Add(new LanguageModel("wa", "wln", "wln", "Walloon"));
        languages.Add(new LanguageModel("cy", "wel", "wel", "Welsh"));
        languages.Add(new LanguageModel("fy", "fry", "fry", "Western Frisian"));
        languages.Add(new LanguageModel("wo", "wol", "wol", "Wolof"));
        languages.Add(new LanguageModel("xh", "xho", "xho", "Xhosa"));
        languages.Add(new LanguageModel("yi", "yid", "yid", "Yiddish"));
        languages.Add(new LanguageModel("yo", "yor", "yor", "Yoruba"));
        languages.Add(new LanguageModel("za", "zha", "zha", "Zhuang"));
        languages.Add(new LanguageModel("zu", "zul", "zul", "Zulu"));
        return languages;
    }

    // Nested, private class
    private class LanguageModel
    {
        public string ISO639_1 { get; set; }
        public string ISO639_2 { get; set; }
        public string ISO639_3 { get; set; }
        public string LanguageLong { get; set; }

        public LanguageModel(string ISO_1, string ISO_2, string ISO_3, string LongName)
        {
            ISO639_1 = ISO_1;
            ISO639_2 = ISO_2;
            ISO639_3 = ISO_3;
            LanguageLong = LongName;
        }
    }
    #endregion
}