using OpenUtau.Plugin.Builtin;
using Xunit;

namespace ChineseToJapanesePhonemizer.Tests;

public class ParsePinyinStandardTests {
    // 普通话标准音节表
    private static readonly string[] StandardSyllables = (
        "a o e ai ei ao ou an en ang eng er " +
        "yi ya yo ye yao you yan yin yang ying yong yu yue yuan yun " +
        "wu wa wo wai wei wan wen wang weng " +
        "ba bai ban bang bao bei ben beng bi bian biao bie bin bing bo bu " +
        "pa pai pan pang pao pei pen peng pi pian piao pie pin ping po pou pu " +
        "ma mai man mang mao me mei men meng mi mian miao mie min ming miu mo mou mu " +
        "fa fan fang fei fen feng fo fou fu " +
        "da dai dan dang dao de dei deng dia dian diao die ding diu dong dou du duan dui dun duo " +
        "ta tai tan tang tao te teng ti tian tiao tie ting tong tou tu tuan tui tun tuo " +
        "na nai nan nang nao ne nei neng ni nian niang niao nie nin ning niu nong nu nuan nuo nv nve " +
        "la lai lan lang lao le lei leng li lia lian liang liao lie lin ling liu lo long lou lu luan lun luo lv lve " +
        "ga gai gan gang gao ge gei gen geng gong gou gu gua guai guan guang gui gun guo " +
        "ka kai kan kang kao ke ken keng kong kou ku kua kuai kuan kuang kui kun kuo " +
        "ha hai han hang hao he hei hen heng hong hou hu hua huai huan huang hui hun huo " +
        "ji jia jian jiang jiao jie jin jing jiong ju juan jue jun " +
        "qi qia qian qiang qiao qie qin qing qiong qu quan que qun " +
        "xi xia xian xiang xiao xie xin xing xiong xu xuan xue xun " +
        "ran rang rao re ren reng ri rong rou ru ruan rui run ruo " +
        "za zai zan zang zao ze zei zen zeng zi zong zou zu zuan zui zun zuo " +
        "ca cai can cang cao ce cen ceng ci cong cou cu cuan cui cun cuo " +
        "sa sai san sang sao se sen seng si song sou su suan sui sun suo " +
        "zha zhai zhan zhang zhao zhe zhen zheng zhi zhong zhou zhu zhua zhuai zhuan zhuang zhui zhun zhuo " +
        "cha chai chan chang chao che chen cheng chi chong chou chu chuai chuan chuang chui chun chuo " +
        "sha shai shan shang shao she shen sheng shi shou shu shua shuai shuan shuang shui shun shuo " +
        "n ng"
    ).Split(' ', StringSplitOptions.RemoveEmptyEntries);

    [Fact]
    public void StandardSyllables_AllParseToNonNull() {
        var failed = StandardSyllables
            .Where(s => Zh2JaCore.ParsePinyin(s) == null)
            .ToList();
        Assert.True(failed.Count == 0,
            "ParsePinyin 对以下标准音节返回 null: " + string.Join(", ", failed));
    }

    [Fact]
    public void Yuan_ParsesAsVanFinal() {
        var parsed = Zh2JaCore.ParsePinyin("yuan");
        Assert.NotNull(parsed);
        Assert.Equal(new[] { "van" }, parsed);
    }

    [Fact]
    public void Yuan_GeneratesYuEN_Moras() {
        var moras = Zh2JaCore.GenerateMoras(
            Array.Empty<string>(), "van", Zh2JaCore.DefaultFullPinyinMap);
        Assert.Equal(new[] { "ゆ", "え", "ん" }, moras);
    }

    [Fact]
    public void YuFamily_ParsesAsVSeriesFinals() {
        Assert.Equal(new[] { "v" }, Zh2JaCore.ParsePinyin("yu"));
        Assert.Equal(new[] { "ve" }, Zh2JaCore.ParsePinyin("yue"));
        Assert.Equal(new[] { "vn" }, Zh2JaCore.ParsePinyin("yun"));
    }
}
