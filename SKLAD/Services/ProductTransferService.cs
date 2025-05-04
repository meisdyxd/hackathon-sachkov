using SKLAD.Entities;

namespace SKLAD.Services
{
    public class ProductTransferService
    {
        public bool CanTransfer(Product product, StorageZone targetZone)
        {
            // тут кароче я хотел писать бизнес логику и выносить ее, но, в ProductService я объяснил почему отказался от этого
            if (product.StorageRequirements == "Danger"
                && targetZone.Type != "Danger")
            {
                return false;
            }
            return true;
        }
    }
}
