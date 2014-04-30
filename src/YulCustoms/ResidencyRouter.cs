namespace YulCustoms
{
    public class ResidencyRouter: IHandle<CustomsDeclaration>{
        private readonly IHandle<CustomsDeclaration> residentHandler;

        private readonly IHandle<CustomsDeclaration> visitorHandler;

        public ResidencyRouter(IHandle<CustomsDeclaration> residentHandler, IHandle<CustomsDeclaration> visitorHandler)
        {
            this.residentHandler = residentHandler;
            this.visitorHandler = visitorHandler;
        }

        public void Handle(CustomsDeclaration message)
        {
            if (message.Resident)
            {
                this.residentHandler.Handle(message);
            }
            else
            {
                this.visitorHandler.Handle(message);
            }

        }
    }
}