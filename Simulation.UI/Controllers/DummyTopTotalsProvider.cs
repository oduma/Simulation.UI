using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simulation.UI.Models;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Web.Mvc;

namespace Simulation.UI.Controllers
{
    public class DummyTopTotalsProvider: DummyTopRecordProvider, ITopTotalsProvider
    {
        public void SaveTotalForItems(TopForTotalModel topTotalForModel, string typeOfTotal, HttpContextBase context)
        {
            List<TotalItem> existingItems = GetTotalItems(typeOfTotal, context).ToList();

            foreach (TopItem topItem in topTotalForModel.TopItems)
            {
                var itemScore = ScoreItem(topItem.Rank);
                if (itemScore > 0)
                {
                    TotalItem existingItem = existingItems.FirstOrDefault(i => i.ItemName == topItem.ItemName);
                    if (existingItem == null)
                    {
                        existingItem = new TotalItem { ItemName = topItem.ItemName, EntryWeek = topTotalForModel.WeekNo.ToString(), Score = itemScore };
                        existingItems.Add(existingItem);
                    }
                    else
                        existingItem.Score += itemScore;
                    RecalculatePositions(existingItems);
                }
                else
                    break;
            }
            TrySaveItems(existingItems, typeOfTotal, context);
            RecordSavedWeek(typeOfTotal, topTotalForModel.WeekNo, context);
        }

        private void RecordSavedWeek(string typeOfTotal, int weekNo, HttpContextBase context)
        {
            List<TopRecordedItem> topRecordedItems = GetTopProcessed(context);
            topRecordedItems.Add(new TopRecordedItem { TypeOfSelection = typeOfTotal, WeekNo = weekNo });
            var fileFullPath = context.Request.MapPath(@"..\..") + @"\toprecord.xml";

            using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TopRecordedItem>));
                serializer.Serialize(fs, topRecordedItems);
                fs.Flush();
                fs.Close();
            }

        }

        private void TrySaveItems(List<TotalItem> existingItems, string typeOfTotal,HttpContextBase context)
        {
            
            var fileFullPath = context.Request.MapPath(@"..\..") + @"\" + typeOfTotal + ".xml";

            using (FileStream fs = new FileStream(fileFullPath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TotalItem>));
                serializer.Serialize(fs, existingItems);
                fs.Flush();
                fs.Close();
            }
        }

        private void RecalculatePositions(List<TotalItem> existingItems)
        {
            existingItems.Sort(new TotalItemComparer());
            for (int i = 0; i < existingItems.Count; i++)
                existingItems[i].Position = i+1;
        }

        private int ScoreItem(int rank)
        {
            switch(rank)
            {
                case 1:
                    return 3;
                case 2:
                    return 2;
                case 3:
                    return 1;
                default:
                    return 0;
            }
        }

        public IEnumerable<TotalItem> GetTotalItems(string typeOfTotal, HttpContextBase context)
        {
            var fileFullPath = context.Request.MapPath(@"..\..") + @"\" + typeOfTotal + ".xml";
            if (!File.Exists(fileFullPath))
                return new List<TotalItem>();
            using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TotalItem>));
                return xmlSerializer.Deserialize(fs) as List<TotalItem>;
            }
        }
    }
}