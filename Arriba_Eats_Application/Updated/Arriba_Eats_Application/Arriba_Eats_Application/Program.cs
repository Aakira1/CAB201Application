using System;
using ArribaEats.Models;
using ArribaEats.Services;
using ArribaEats.UI;

namespace ArribaEats
{
    class Program
    {
        static void Main(string[] args)
        {
            IArribaEatsService service = new ArribaEatsService();
            
            ArribaEatsUI ui = new ArribaEatsUI(service); // Create the UI that depends on the service
            ui.Run();
        }
    }
}