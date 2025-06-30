namespace Module.Identity.Infrastructure.Services.OTP
{
    public class SecurityToken(byte[] data)
    {
        private readonly byte[] _data = (byte[])data.Clone();

        internal byte[] GetDataNoClone()
        {
            return _data;
        }
    }
}
