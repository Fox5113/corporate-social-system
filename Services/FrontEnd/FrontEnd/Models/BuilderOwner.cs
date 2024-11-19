namespace FrontEnd.Models
{
    public class BuilderOwner
    {
        UserDataBuilder builder;
        public BuilderOwner(UserDataBuilder builder)
        {
            this.builder = builder;
        }
        public void Construct()
        {
            builder.SetLogin();
            builder.BuildFromUserModel();
            builder.SetLanguage();
            builder.SetAdditional();
        }
    }
}
