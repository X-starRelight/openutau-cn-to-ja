using OpenUtau.Plugin.Builtin;
using YamlDotNet.Serialization;

namespace ChineseToJapanesePhonemizer.Tests;

public class Zh2JaConfigTests {

    private static Zh2JaConfig? Deserialize(string yaml) {
        var deserializer = new DeserializerBuilder().Build();
        return deserializer.Deserialize<Zh2JaConfig>(yaml);
    }

    [Fact]
    public void Defaults_MatchDocumentedValues() {
        var cfg = Deserialize("nasal_mode: short");
        Assert.NotNull(cfg);
        Assert.Equal("short", cfg!.NasalMode);
        Assert.Equal(120, cfg.NasalMs);
        Assert.True(cfg.UseWildcard);
        Assert.False(cfg.DisableVcv);
        Assert.False(cfg.DisableLightTone);
        Assert.False(cfg.DisableToneTiming);
        Assert.False(cfg.DisableDoubleChar);
        Assert.False(cfg.AskPolyphone);
        Assert.False(cfg.DisableTone3);
        Assert.Empty(cfg.FullPinyin);
        Assert.Empty(cfg.Polyphones);
        Assert.Empty(cfg.AliasOverrides);
    }

    [Fact]
    public void AllKeys_AreDeserialized() {
        var yaml = """
            nasal_mode: none
            nasal_ms: 250
            use_wildcard: false
            disable_vcv: true
            disable_light_tone: true
            disable_tone_timing: true
            disable_double_char: true
            ask_polyphone: true
            disable_tone3: true
            """;
        var cfg = Deserialize(yaml);
        Assert.NotNull(cfg);
        Assert.Equal("none", cfg!.NasalMode);
        Assert.Equal(250, cfg.NasalMs);
        Assert.False(cfg.UseWildcard);
        Assert.True(cfg.DisableVcv);
        Assert.True(cfg.DisableLightTone);
        Assert.True(cfg.DisableToneTiming);
        Assert.True(cfg.DisableDoubleChar);
        Assert.True(cfg.AskPolyphone);
        Assert.True(cfg.DisableTone3);
    }

    [Fact]
    public void CustomPinyinAndPolyphones_AreDeserialized() {
        var yaml = """
            full_pinyin:
              hua: [fa]
              chi: [chi]
            polyphones:
              "chang da": "zhang da"
            """;
        var cfg = Deserialize(yaml);
        Assert.NotNull(cfg);
        Assert.Equal(new[] { "fa" }, cfg!.FullPinyin["hua"]);
        Assert.Equal(new[] { "chi" }, cfg.FullPinyin["chi"]);
        Assert.Equal("zhang da", cfg.Polyphones["chang da"]);
    }

    [Fact]
    public void AliasOverrides_AreDeserialized() {
        var yaml = """
            alias_overrides:
              "ん": ["ん -", "n -", "n"]
            """;
        var cfg = Deserialize(yaml);
        Assert.NotNull(cfg);
        Assert.Equal(new[] { "ん -", "n -", "n" }, cfg!.AliasOverrides["ん"]);
    }

    [Fact]
    public void EmptyDocument_ReturnsNull() {
        Assert.Null(Deserialize(""));
    }
}
