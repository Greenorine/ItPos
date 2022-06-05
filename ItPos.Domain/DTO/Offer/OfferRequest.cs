namespace ItPos.Domain.DTO.Offer;

public class OfferRequest
{
    public Guid? Id { get; set; }
    public string Title { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public string Activity { get; set; }
    public string DeliveryWay { get; set; }
}