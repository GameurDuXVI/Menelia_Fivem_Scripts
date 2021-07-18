
namespace Menelia.Client.CallbackClasses
{
    public class CallbackClass
    {
        public bool HasResponse = false;
    }
    public class PermissionCallback : CallbackClass
    {
        public bool Permission = false;
    }

    public class PlayerInfoCallback : CallbackClass
    {
        public string Json = "";
    }
}
