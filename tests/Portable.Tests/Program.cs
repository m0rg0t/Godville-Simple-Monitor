using System;
using System.Globalization;
using GodvilleSimpleMonitor;

class Program
{
    static int failures;
    static void Check(string name, Action test)
    {
        try { test(); Console.WriteLine("PASS: " + name); }
        catch (Exception error) { failures++; Console.WriteLine("FAIL: " + name + ": " + error.Message); }
    }
    static void Assert(bool condition) { if (!condition) throw new Exception("Assertion failed"); }
    static int Main()
    {
        var page = new MainPage();
        Check("adjacent inventory elements", () => Assert(
            page.TestParse("item", "<root><item>one</item><item>two</item><item>three</item></root>") ==
            "one\r\ntwo\r\nthree\r\n"));
        Check("separated inventory elements", () => Assert(
            page.TestParse("item", "<root><item>one</item><other/><item>two</item></root>") ==
            "one\r\ntwo\r\n"));
        Check("missing element", () => Assert(page.TestParse("item", "<root/>") == ""));
        Check("malformed XML preserves fallback", () => Assert(page.TestParse("item", "<bad") == "0"));
        foreach (var cultureName in new[] { "en-US", "ru-RU" })
        {
            var culture = cultureName;
            Check("health reflects response in " + culture, () => {
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                page.TestResponse("<root><health>12.5</health><max_health>100.5</max_health></root>");
                Assert(page.HealthBar.Value == 12.5 && page.HealthBar.Maximum == 100.5);
            });
        }
        return failures == 0 ? 0 : 1;
    }
}
