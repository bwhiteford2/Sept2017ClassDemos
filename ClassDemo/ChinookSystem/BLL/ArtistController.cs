﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Chinook.Data.Entities;
using ChinookSystem.DAL;
using System.ComponentModel; // Expose methods for ODS (Object Data Source) wizard
using Chinook.Data.POCOs;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject] // annotate the class
    public class ArtistController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)] // annotate the method
        public List<Artist> Artists_List()
        {
            using (var context = new ChinookContext())
            {
                return context.Artists.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> List_ArtistNames()
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Artists
                              orderby x.Name
                              select new SelectionList
                              {
                                  IDValueField = x.ArtistId,
                                  DisplayText = x.Name
                              };
                return results.ToList();
            }
        }
    }
}
