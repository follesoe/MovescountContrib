using DDay.iCal.Serialization;
using DDay.iCal.Serialization.iCalendar;
using MovescountContrib.Lib;
using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;

namespace MovescountContrib.Web.Controllers
{
    public class CalendarController : Controller
    {
        public ActionResult ICalFeed(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Email missing.");
            }

            var password = ConfigurationManager.AppSettings[email];

            if (String.IsNullOrWhiteSpace(password))
            {
                throw new Exception(string.Format("User {0} does not exist.", email));
            }

            var iCal = PlannedMoves.getTrainingPlanCal(email, password);

            ISerializationContext ctx = new SerializationContext();
            ISerializerFactory factory = new SerializerFactory();
            var serializer = (IStringSerializer)factory.Build(iCal.GetType(), ctx);

            string output = serializer.SerializeToString(iCal);
            return File(Encoding.UTF8.GetBytes(output), "text/calendar", "Movescount.ical");
        }
    }
}