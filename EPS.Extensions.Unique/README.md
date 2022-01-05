# EPS.Extensions.Unique

What initially started as a way to replace System.Web.Security.Membership.GeneratePassword from legacy ASP.NET has turned into a unique ID generator that's shorter than using GUIDs. You can use it to build identifiers similar to YouTube or Firestore IDs, if needed. We ran a test building identifiers with 7 alphanumeric characters (and no punctuation characters) and managed to get 10 million unique identifiers out of it. We didn't try a larger number, but, assuming our math isn't wrong (🤷), it seems you should be able to get at least 62^7 or 3.522 trillion unique identifiers out of it.

Sample output:

```c#
[Fact]
public void TestUnique()
{
    var hs = new HashSet<string>();
    for (int i = 0; i < 100; i++)
    {
        var u = Unique.Unique.Generate(8, 0);
        hs.Add(u);
        output.WriteLine(u);
    }
}
Xq8Lnhkq
udPe7BhM
mvsKFYjy
IiQrpozS
hcbskLr4
VmsINq1M
O2rMu84n
f6sHbtk1
XbDcLacW
idhJKidB
xAgditbO
TuQCrY3U
...
```

