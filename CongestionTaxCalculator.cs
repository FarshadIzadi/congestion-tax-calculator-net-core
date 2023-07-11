using System;
using congestion.calculator;
using static congestion.calculator.GlobalConstants;

public class CongestionTaxCalculator
{
    /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total congestion tax for that day
         */

    public int GetTax(Vehicle vehicle, DateTime[] dates)
    {
        int totalFee = 0;

        for (int i = 0; i < dates.Length; i++)
        {
            TimeSpan Timediff = new TimeSpan();
            DateTime intervalStart = dates[i];
            double DiffInMinutes = 0;
            int currentFee = GetTollFee(dates[i], vehicle);
            
            if(i<dates.Length - 1)
            {
                Timediff = dates[i+1] - intervalStart;
                DiffInMinutes = Timediff.TotalMinutes;

            }

            while(DiffInMinutes <= 60 && i<dates.Length)
            {
                i++;
                currentFee = Math.Max(currentFee, GetTollFee(dates[i], vehicle));
                Timediff = dates[i] - intervalStart;
                DiffInMinutes = Timediff.TotalMinutes;
            }
            totalFee += currentFee;
            
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Bus.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute <= 29) return 8;
        else if (hour == 6) return 13;
        else if (hour == 7) return 18;
        else if (hour == 8 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14 ) return 8;
        else if (hour == 15 && minute <= 29) return 13;
        else if (hour == 15 || hour == 16 ) return 18;
        else if (hour == 17 ) return 13;
        else if (hour == 18 && minute <= 29) return 8;
        else return 0;
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if ((month == 1 && day == 1) ||
                (month == 3 && (day == 28 || day == 29)) ||
                (month == 4 && (day == 1 || day == 30)) ||
                (month == 5 && (day == 1 || day == 8 || day == 9)) ||
                (month == 6 && (day == 5 || day == 6 || day == 21)) ||
                (month == 7) ||
                (month == 11 && day == 1) ||
                (month == 12 && (day == 24 || day == 25 || day == 26 || day == 31)))
            {
                return true;
            }
        }
        return false;
    }


}