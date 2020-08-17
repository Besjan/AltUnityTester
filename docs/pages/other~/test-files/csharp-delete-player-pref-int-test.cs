[Test]
public void TestDeleteKey()
{
    altUnityDriver.DeletePlayerPref();
    altUnityDriver.SetKeyPlayerPref("test", 1);
    var val = altUnityDriver.GetIntKeyPlayerPref("test");
    Assert.AreEqual(1, val);
    altUnityDriver.DeleteKeyPlayerPref("test");
    try
    {
        altUnityDriver.GetIntKeyPlayerPref("test");
        Assert.Fail();
    }
    catch (NotFoundException exception)
    {
        Assert.AreEqual("error:notFound", exception.Message);
    }

}