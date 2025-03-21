using System;

namespace MusicStore.Models
{
    // Базовый класс для всех моделей
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public int RecordId { get; set; }

        // Валидация записи
        public virtual bool IsValid()
        {
            return Id > 0 && RecordId > 0;
        }
    }

    // Класс продажи записи
    public class Sale : BaseModel
    {
        private DateTime _saleDate;
        private decimal _price;

        public DateTime SaleDate
        {
            get => _saleDate;
            set
            {
                if (value < DateTime.Now.AddYears(-100))
                    throw new ArgumentException("Дата продажи не может быть слишком старой");
                _saleDate = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Цена не может быть отрицательной");
                _price = value;
            }
        }

        public string CustomerName { get; set; }

        // Интерфейс для операций с продажами
        public interface ISaleOperations
        {
            decimal CalculateRevenue();
            bool IsRecentSale(int daysThreshold);
        }

        public decimal CalculateRevenue()
        {
            return Price;
        }

        public bool IsRecentSale(int daysThreshold)
        {
            return (DateTime.Now - SaleDate).Days <= daysThreshold;
        }
    }

    // Класс операции с запасом
    public class StockOperation : BaseModel
    {
        private DateTime _operationDate;

        public DateTime OperationDate
        {
            get => _operationDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Дата операции не может быть в будущем");
                _operationDate = value;
            }
        }

        public string OperationType { get; set; } // "Поставка"/"Продажа"/"Потери"
        public int Quantity { get; set; }
        public string Reason { get; set; }

        // Валидация типа операции
        public bool IsValidOperationType()
        {
            return OperationType == "Поставка" ||
                   OperationType == "Продажа" ||
                   OperationType == "Потери";
        }
    }

    // Класс акции
    public class Promotion : BaseModel
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private decimal _discountPercentage;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value > EndDate)
                    throw new ArgumentException("Дата начала не может быть позже даты окончания");
                _startDate = value;
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value < StartDate)
                    throw new ArgumentException("Дата окончания не может быть раньше даты начала");
                _endDate = value;
            }
        }

        public decimal DiscountPercentage
        {
            get => _discountPercentage;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Скидка должна быть от 0 до 100%");
                _discountPercentage = value;
            }
        }

        public string Description { get; set; }

        // Интерфейс для операций с акциями
        public interface IPromotionOperations
        {
            bool IsPromotionActive(DateTime date);
            decimal CalculateDiscountedPrice(decimal originalPrice);
        }

        public bool IsPromotionActive(DateTime date)
        {
            return date >= StartDate && date <= EndDate;
        }

        public decimal CalculateDiscountedPrice(decimal originalPrice)
        {
            return originalPrice * (1 - DiscountPercentage / 100);
        }
    }

    // Класс забронированной записи
    public class ReservedRecord : BaseModel
    {
        private DateTime _reserveDate;
        private DateTime _expireDate;

        public DateTime ReserveDate
        {
            get => _reserveDate;
            set
            {
                if (value > ExpireDate)
                    throw new ArgumentException("Дата бронирования не может быть позже даты истечения");
                _reserveDate = value;
            }
        }

        public DateTime ExpireDate
        {
            get => _expireDate;
            set
            {
                if (value < ReserveDate)
                    throw new ArgumentException("Дата истечения не может быть раньше даты бронирования");
                _expireDate = value;
            }
        }

        public string CustomerName { get; set; }
        public bool IsConfirmed { get; set; }

        // Проверка актуальности бронирования
        public bool IsReservationActive()
        {
            return !IsConfirmed && DateTime.Now <= ExpireDate;
        }
    }

    // Класс логин и пароль
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}