using System.ComponentModel;
using AlertsAdmin.Domain.Attributes;


namespace AlertsAdmin.Domain.Enums
{
    [QueueTable]
    public enum PencilQueueTable
    {
        [QueueName("Pencil OrderToDispatch")]
        [Query("SELECT COUNT(*) FROM PENCIL.dbo.RunQueueOrderToDispatch WITH (NOLOCK) WHERE RequiresAttention = 0")]
        PencilOrderToDispatch,
        [QueueName("Pencil Forms Building")]
        [Query("SELECT COUNT(*) FROM PENCIL.dbo.RunRequestForm WITH (NOLOCK) WHERE Built = 0")]
        PencilFormsBuilding,
        [QueueName("Pencil Automation")]
        [Query("SELECT COUNT(*) FROM PENCIL.dbo.OrderLock WITH (NOLOCK) WHERE UserName = 'Automation'")]
        PencilAutomation,
        [QueueName("Pencil Order Updates")]
        [QueueResource("[//PENCIL/OrderUpdateRequestTargetQueue]")]
        PencilOrderUpdates,
        [QueueName("Pencil Create")]
        [QueueResource("[//PENCIL/OrderCreateRequestTargetQueue]")]
        PencilOrderCreateRequestTargetQueue,
        [QueueName("Maple Order Updates")]
        [Query("SELECT COUNT(*) FROM Maple.dbo.[//Maple/OrderUpdateRequestTargetQueue]")]
        MapleOrderUpdateRequestTargetQueue
    }
}
