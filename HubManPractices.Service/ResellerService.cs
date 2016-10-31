using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubManPractices.Models;
using HubManPractices.Repository;
using HubManPractices.Repository.Repositories;
using AutoMapper;
using HubManPractices.Service.ViewModels;

namespace HubManPractices.Service
{
    public class ResellerService : IResellerService
    {
        private readonly IResellerRepository resellersRepository;

        private readonly ApplicationEntities ApplicationEntity;

        public ResellerService(ResellerRepository RR)
        {
            this.resellersRepository = RR;
        }
        public ResellerService(ApplicationEntities Ae, IResellerRepository RR)
        {
            this.resellersRepository = RR;
            this.ApplicationEntity = Ae;
        }

        public IEnumerable<Reseller> GetResellers()
        {
            return resellersRepository.GetMany(r => r.IsDeleted == false);
        }
        public void SaveReseller()
        {
            ApplicationEntity.Commit();
        }

        public IEnumerable<ResellerViewModel> MapToViewModel(IEnumerable<Reseller> resellers)
        {
           return Mapper.Map<IEnumerable<Reseller>, IEnumerable<ResellerViewModel>>(resellers);
        }

        public IEnumerable<Reseller> GetUserReseller(string UserID)
        {
            return resellersRepository.GetUserReseller(UserID);
        }

        public void CreateReseller(Reseller r)
        {
            resellersRepository.Add(r);
        }


        public ResellerViewModel MapToViewModel(Reseller reseller)
        {
            return Mapper.Map<Reseller,ResellerViewModel>(reseller);
        }

        public string GetResellerImage(Guid ResellerID)
        {
            return resellersRepository.GetResellerImage(ResellerID);
        }


        public Reseller GetById(Guid ResellerID)
        {
            return resellersRepository.GetById(ResellerID);
        }


        public void EditReseller(Reseller reseller)
        {
            resellersRepository.Update(reseller);
        }


        public void DeleteReseller(Reseller ToBeDeleted)
        {
            resellersRepository.Delete(ToBeDeleted);
        }


        public IEnumerable<Reseller> SearchForResellers(string Query)
        {
            return resellersRepository.SearchForResellers(Query);
        }


        public IEnumerable<Client> GetResellerClients(Guid ResellerID)
        {
            return resellersRepository.GetResellerClients(ResellerID);
        }


        public bool QuotaFull(Guid ResellerID)
        {
            return resellersRepository.QuotaFull(ResellerID);
        }

        public IEnumerable<Client> GetResellerDeletedClients(Guid resellerID)
        {
            return resellersRepository.GetResellerDeletedClients(resellerID);
        }

        public IEnumerable<Client> GetRecent10Clients(IEnumerable<Client> clients)
        {
            List <Client> myList = clients.ToList();
            List <Client> ordered = myList.OrderByDescending(m => m.CreationDate).ToList();
            return ordered.Take(3);
        }
        public int GetQuarter(int CurrentMonth)
        {
            if (CurrentMonth >= 1 && CurrentMonth <= 3)
            {
                return 1;
            }
            else if (CurrentMonth >= 4 && CurrentMonth <= 6)
            {
                return 2;
            }
            else if (CurrentMonth >= 7 && CurrentMonth <= 9)
            {
                return 3;
            }
            else return 4;
        }
        public Dictionary<string,int> GetChartData(Guid ResellerID)
        {
            int CurrentYear = DateTime.Now.Year;
            int CurrentMonth = DateTime.Now.Month;
            int Quarter = GetQuarter(CurrentMonth);
            int StartChart;
            switch (Quarter)
            {
                case 1:
                    StartChart = 1;
                    break;
                case 2:
                    StartChart = 4;
                    break;
                case 3:
                    StartChart = 7;
                    break;
                default:
                    StartChart = 10;
                    break;
            }
            int EndChart =StartChart+2;
            List<Client> AllResellerClients = GetResellerClients(ResellerID).ToList();
            var GroupedData = AllResellerClients.Where(m=>m.CreationDate.Month >= StartChart && m.CreationDate.Month <= EndChart && m.CreationDate.Year==CurrentYear).OrderBy(m=>m.CreationDate.Month).GroupBy(x => x.CreationDate.Month).ToList();
            Dictionary<string,int> chartdata = new Dictionary<string, int>();
            foreach (var item in GroupedData)
            {
                string month = item.FirstOrDefault().CreationDate.ToString("MMMM");
                chartdata[month] = item.Count();
            }
            Dictionary<int, int> BeforeSort= new Dictionary<int, int>();
            if(chartdata.Count()!=3)
            {
                for(int i=StartChart; i<=EndChart;i++)
                {
                    DateTime date = new DateTime(2016, i, 10);
                    if(!chartdata.ContainsKey(date.ToString("MMMM")))
                    {
                        BeforeSort.Add(date.Month, 0);
                    }
                    else
                    {
                        BeforeSort.Add(i, chartdata[date.ToString("MMMM")]);
                    }
                }
            }
            var AfterSort = BeforeSort.OrderBy(key => key.Key);
            chartdata.Clear();
            for(int i=0;i<AfterSort.Count();i++)
            {
                DateTime date = new DateTime(2016, AfterSort.ElementAt(i).Key , 10);
                chartdata.Add(date.ToString("MMMM"), AfterSort.ElementAt(i).Value);
            }
            return chartdata;
        }

        public void AddResellerUser(Reseller r, string UserMail)
        {
            resellersRepository.AddResellerUser(r, UserMail);
        }
    }
}
