using OpenUtau.Plugin.Builtin;

namespace ChineseToJapanesePhonemizer.Tests;

public class KanaConversionTests {

    private static Dictionary<string, string[]> DefaultMap() {
        var map = new Dictionary<string, string[]>();
        foreach (var kv in Zh2JaCore.DefaultFullPinyinMap) map[kv.Key] = kv.Value;
        return map;
    }

    [Theory]
    [InlineData("ka", "か")]
    [InlineData("shi", "し")]
    [InlineData("byo", "びょ")]
    [InlineData("n", "ん")]
    [InlineData("unknown", "unknown")]
    [InlineData("", "")]
    public void ToKana_TranslatesRomaji(string romaji, string expected) {
        Assert.Equal(expected, Zh2JaCore.ToKana(romaji));
    }

    // 文档示例：早 (zao) → ざ お
    [Fact]
    public void GenerateMoras_Zao_GivesTwoMoras() {
        var moras = Zh2JaCore.GenerateMoras(new[] { "z" }, "ao", DefaultMap());
        Assert.Equal(new[] { "ざ", "お" }, moras);
    }

    // 文档示例：喵 (miao) → 拗音 + 单韵母
    [Fact]
    public void GenerateMoras_Miao_UsesYouon() {
        var moras = Zh2JaCore.GenerateMoras(new[] { "m" }, "iao", DefaultMap());
        Assert.Equal(new[] { "みゃ", "お" }, moras);
    }

    // 文档示例：穿 (chuan) → ちゅ あ ん
    [Fact]
    public void GenerateMoras_Chuan_ThreeMoras() {
        var moras = Zh2JaCore.GenerateMoras(new[] { "ch" }, "uan", DefaultMap());
        Assert.Equal(new[] { "ちゅ", "あ", "ん" }, moras);
    }

    // 文档示例：我 (wo) → うぉ
    [Fact]
    public void GenerateMoras_Wo_FallsBackToVowelSequence() {
        var moras = Zh2JaCore.GenerateMoras(new[] { "w" }, "o", DefaultMap());
        Assert.Equal(new[] { "うぉ" }, moras);
    }

    [Fact]
    public void GenerateMoras_FallbackPath_MapsInitialAndSplitsVowels() {
        // "bia" 不在 FullPinyinMap 中 → 走 InitialMap + 元音序列回退
        var moras = Zh2JaCore.GenerateMoras(new[] { "b" }, "ia", DefaultMap());
        Assert.Equal(new[] { "び", "あ" }, moras);
    }

    [Fact]
    public void GenerateMoras_NasalEnding_EmitsN() {
        // "fian" 不在 FullPinyinMap 中 → f→h，ian → i e N
        var moras = Zh2JaCore.GenerateMoras(new[] { "f" }, "ian", DefaultMap());
        Assert.Equal(new[] { "ひ", "え", "ん" }, moras);
    }

    [Fact]
    public void GenerateMoras_UnmappedPinyin_FallsBackToRawMora() {
        // FullPinyinMap 未命中且 GetJapaneseVowelSequence 的 default 分支
        // 原样返回韵母 → 产出原始字符串。管线中 ParsePinyin 只会产出
        // Finals 内的韵母（全部有映射），此路径实际不可达。
        var moras = Zh2JaCore.GenerateMoras(new[] { "b" }, "qqq", DefaultMap());
        Assert.Equal(new[] { "bqqq" }, moras);
    }

    [Theory]
    [InlineData("", "-")]
    [InlineData("-", "-")]
    [InlineData("R", "R")]
    [InlineData("a", "a")]
    [InlineData("an", "n")]
    [InlineData("ou", "o")]
    [InlineData("v", "u")]
    [InlineData("er", "a")]
    public void ConvertToJapaneseVowel_ReturnsTrailingVowel(string pinyinFinal, string expected) {
        Assert.Equal(expected, Zh2JaCore.ConvertToJapaneseVowel(pinyinFinal));
    }

    [Theory]
    [InlineData("", "-")]
    [InlineData("あ", "a")]
    [InlineData("ん", "n")]
    [InlineData("きゃ", "a")]
    [InlineData("し", "i")]
    [InlineData("ふ", "u")]
    [InlineData("べ", "e")]
    [InlineData("と", "o")]
    [InlineData("！", "-")]
    public void GetLastJapaneseVowel_MapsKanaToVowel(string mora, string expected) {
        Assert.Equal(expected, Zh2JaCore.GetLastJapaneseVowel(mora));
    }
}
