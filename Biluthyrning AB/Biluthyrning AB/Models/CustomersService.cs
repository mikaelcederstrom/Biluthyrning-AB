using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;

namespace Biluthyrning_AB.Models
{
    public class CustomersService
    {

        public CustomersService(ICustomersRepository customersRepository, EventsService eventsService)
        {
            this.customersRepository = customersRepository;
            this.eventsService = eventsService;
        }
        private readonly ICustomersRepository customersRepository;
        private readonly EventsService eventsService;

        internal void AddCustomerToDB(CustomersAddVM viewModel)
        {
            Customers customer = new Customers
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PersonNumber = viewModel.PersonNumber,
                KilometersDriven = 0,
                NumberOfOrders = 0,
                MembershipLevel = 0
            };

            customersRepository.Add(customer);
            eventsService.CreateAddedCustomerEvent(customer);
        }
        internal CustomersListOfAllVM[] GetAllCustomersFromDB()
        {
            return customersRepository.GetAllCustomers().ToArray();
        }
        internal CustomersDetailsVM GetCustomerDetailsFromDB(int id)
        {
            return customersRepository.GetCustomerDetails(id);
        }

        internal int GetCustomersIdFromPersonNumber(string personNumber)
        {
            return customersRepository.GetIdFromPersonNumber(personNumber);
        }

        internal int GetMembershipLevelByID(int id)
        {
            return customersRepository.GetMembershipLevelByID(id);
        }
    }
}
