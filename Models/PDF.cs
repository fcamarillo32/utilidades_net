namespace n8n_urilisSWA.Models
{
    //esto es para el pedido del cliente por clientes
    public class Pedido
    {
        public string Numero { get; set; }
        public string Nombre_Cliente { get; set; }
        public string Direccion { get; set; }
        public string Fecha { get; set; }
        public List<Productos> Productos { get; set; } = new();
    }

    public class Productos
    {
        public string Producto { get; set; }
        public double Cantidad { get; set; }
        public string Unidad { get; set; }
    }

    //Esto es para el pedido que se realiza
    public class ProductoPedido
    {
        public string Numero { get; set; }
        public string Producto { get; set; }
        public double Cantidad { get; set; }
        public string Unidad { get; set; }
    }

    public class PedidoPdf
    {
        public List<ProductoPedido> Pedido { get; set; }
        public string Fecha { get; set; }
    }


}
