using OpenUtau.Plugin.Builtin;

namespace ChineseToJapanesePhonemizer.Tests;

public class ParsePinyinTests {

    [Fact]
    public void Empty_ReturnsNull() {
        Assert.Null(Zh2JaCore.ParsePinyin(""));
        Assert.Null(Zh2JaCore.ParsePinyin("!!!"));
    }

    [Theory]
    [InlineData("ni", new[] { "n", "i" })]
    [InlineData("hao", new[] { "h", "ao" })]
    [InlineData("zao", new[] { "z", "ao" })]
    [InlineData("a", new[] { "a" })]
    [InlineData("lv", new[] { "l", "v" })]
    [InlineData("zhong", new[] { "zh", "ong" })]
    [InlineData("chuan", new[] { "ch", "uan" })]
    public void SyllableWithInitial_SplitsIntoInitialAndFinal(string pinyin, string[] expected) {
        Assert.Equal(expected, Zh2JaCore.ParsePinyin(pinyin));
    }

    [Theory]
    [InlineData("n", new[] { "N" })]
    [InlineData("ng", new[] { "N" })]
    public void NasalOnly_MapsToN(string pinyin, string[] expected) {
        Assert.Equal(expected, Zh2JaCore.ParsePinyin(pinyin));
    }

    [Theory]
    [InlineData("yi", new[] { "i" })]
    [InlineData("wu", new[] { "u" })]
    [InlineData("yu", new[] { "v" })]
    [InlineData("yin", new[] { "in" })]
    public void YInitial_RemapsToFinal(string pinyin, string[] expected) {
        Assert.Equal(expected, Zh2JaCore.ParsePinyin(pinyin));
    }

    [Theory]
    [InlineData("wang", new[] { "w", "ang" })]
    [InlineData("wen", new[] { "w", "en" })]
    public void WInitial_KeepsWConsonant(string pinyin, string[] expected) {
        Assert.Equal(expected, Zh2JaCore.ParsePinyin(pinyin));
    }

    [Fact]
    public void NonPinyinWords_ReturnNull() {
        Assert.Null(Zh2JaCore.ParsePinyin("nihao"));
        Assert.Null(Zh2JaCore.ParsePinyin("xyzzy"));
    }
}
