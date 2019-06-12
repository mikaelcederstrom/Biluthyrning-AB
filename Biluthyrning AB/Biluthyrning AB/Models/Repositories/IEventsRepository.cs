using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models
{
    public interface IEventsRepository
    {
        EventsIndexVM[] GetAllEvents();
        void SaveEvent(Events e);
    }
}
