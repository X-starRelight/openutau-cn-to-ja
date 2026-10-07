using OpenUtau.Plugin.Builtin;

namespace ChineseToJapanesePhonemizer.Tests;

public class TextProcessingTests {

    [Theory]
    [InlineData("你好，世界！", "你好 世界")]
    [InlineData("a.b;c!", "a b c")]
    [InlineData("（括号）", "括号")]
    [InlineData("纯中文", "纯中文")]
    public void ReplacePunctuation_MapsToSpacesAndTrims(string input, string expected) {
        Assert.Equal(expected, Zh2JaCore.ReplacePunctuation(input));
    }

    [Theory]
    [InlineData("5", "wu")]
    [InlineData("12", "shi er")]
    [InlineData("20", "er shi")]
    [InlineData("25", "er shi wu")]
    [InlineData("2024", "er ling er si")]
    public void ReplaceDigits_ConvertsNumberSequences(string input, string expected) {
        Assert.Equal(expected, Zh2JaCore.ReplaceDigits(input));
    }

    [Theory]
    [InlineData(new[] { 5 }, "wu")]
    [InlineData(new[] { 1, 2 }, "shi er")]
    [InlineData(new[] { 2, 0 }, "er shi")]
    [InlineData(new[] { 2, 5 }, "er shi wu")]
    [InlineData(new[] { 1, 0, 0 }, "yi ling ling")]
    public void ConvertDigitSequence_HandlesRanges(int[] digits, string expected) {
        Assert.Equal(expected, Zh2JaCore.ConvertDigitSequence(digits.ToList()));
    }

    [Theory]
    [InlineData("こーぼー", "こぼ")]
    [InlineData("がー", "が")]
    [InlineData("无长音", "无长音")]
    public void StripLongMarks_RemovesChouonpu(string input, string expected) {
        Assert.Equal(expected, Zh2JaCore.StripLongMarks(input));
    }

    [Theory]
    [InlineData("yi2", "yi")]
    [InlineData("bu4", "bu")]
    [InlineData("noprefix", "noprefix")]
    public void StripDigits_RemovesAsciiDigits(string input, string expected) {
        Assert.Equal(expected, Zh2JaCore.StripDigits(input));
    }

    [Theory]
    [InlineData("mā", "ma", 1)]
    [InlineData("nǚ", "nv", 3)]
    [InlineData("ma5", "ma", 5)]
    [InlineData("ma1", "ma", 1)]
    [InlineData("ma", "ma", 0)]
    [InlineData("lüe", "lve", 0)]
    public void ParseToneMarks_ExtractsTone(string input, string expectedText, int expectedTone) {
        var (text, tone) = Zh2JaCore.ParseToneMarks(input);
        Assert.Equal(expectedText, text);
        Assert.Equal(expectedTone, tone);
    }

    [Theory]
    [InlineData("こんにちは", true)]
    [InlineData("カタカナ", true)]
    [InlineData("ni hao", false)]
    [InlineData("zao", false)]
    public void ContainsJapaneseKana_DetectsKanaRanges(string input, bool expected) {
        Assert.Equal(expected, Zh2JaCore.ContainsJapaneseKana(input));
    }
}
