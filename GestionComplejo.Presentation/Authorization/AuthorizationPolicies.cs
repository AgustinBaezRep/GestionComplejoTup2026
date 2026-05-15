namespace GestionComplejo.Presentation.Authorization
{
    public enum AuthorizationPolicy
    {
        SoloAdmin,
        SoloCliente
    }

    public static class Policies
    {
        public const string SoloAdmin = nameof(AuthorizationPolicy.SoloAdmin);
        public const string SoloCliente = nameof(AuthorizationPolicy.SoloCliente);
    }
}
