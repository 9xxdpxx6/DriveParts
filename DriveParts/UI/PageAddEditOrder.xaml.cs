using DriveParts.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace DriveParts.UI
{
    /// <summary>
    /// Логика взаимодействия для PageAddEditOrder.xaml
    /// </summary>
    public partial class PageAddEditOrder : Page
    {
        private Order _order = new Order();
        private List<Purchase> _purchase = new List<Purchase>();

        public PageAddEditOrder(Order order)
        {
            InitializeComponent();

            AddEditManager.ListOrderItems = ListOrderItems;
            AddEditManager.TextPurchaseTotal = TextPurchaseTotal;
            AddEditManager.TextSubTotal = TextSubTotal;
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            ComboStatus.ItemsSource = data.OrderStatus.ToList();
            ComboCustomer.ItemsSource = data.Customer.ToList();
            ComboDelivery.ItemsSource = data.Delivery.ToList();
            ComboPaymentMethod.ItemsSource = data.PaymentMethod.ToList();

            if (order != null)
            {
                Title = "Редактирование товара";
                _order = order;

                if (order.Paid)
                    TextPaidDate.Visibility = Visibility.Visible;
                else
                    TextPaidDate.Visibility = Visibility.Hidden;

                BtnSaveDocument.Visibility = Visibility.Visible;

                AddEditManager.UpdateTotals(order);
            }

            _order.StatusId = 1;
            _order.Date = DateTime.Today;
            AddEditManager.OrderItems = _purchase;
            DataContext = _order;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            StringBuilder errors = new StringBuilder();

            if (_order.Customer == null)
                errors.AppendLine("Укажите покупателя");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _order.TotalPrice = AddEditManager.SubTotal;

            if (_order.Id == 0)
                data.Order.Add(_order);

            if (_purchase.Count > 0 || _purchase.Count != _order.Purchase.Count)
            {
                try
                {
                    foreach (Purchase oldOrderItem in _order.Purchase.ToList())
                        if (!_purchase.Contains(oldOrderItem))
                            data.Purchase.Remove(oldOrderItem);
                    foreach (Purchase newOrderItem in _purchase)
                        if (!_order.Purchase.Contains(newOrderItem))
                        {
                            newOrderItem.OrderId = _order.Id;
                            data.Purchase.Add(newOrderItem);
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось добавить товар\n" + ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            try
            {
                data.SaveChanges();
                MessageBox.Show("Информация сохранена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                UpdateOrderItems();
        }
        private void UpdateOrderItems()
        {
            ListOrderItems.Items.Clear();
            List<Purchase> allOrderItems = DrivePartsEntities.GetContext().Purchase.Where(p => p.OrderId == _order.Id).ToList();
            foreach (Purchase purchase in allOrderItems)
                ListOrderItems.Items.Add(new UC.UCOrderItem(purchase, null));
        }

        private void BtnSaveDocument_Click(object sender, RoutedEventArgs e)
        {
            DrivePartsEntities data = DrivePartsEntities.GetContext();
            List<Purchase> allPurchases = data.Purchase.Where(p => p.OrderId == _order.Id).ToList();

            Word.Application application = new Word.Application();

            Word.Document document = application.Documents.Add();

            Word.Paragraph orderParagraph = document.Paragraphs.Add();
            Word.Range orderRange = orderParagraph.Range;
            orderRange.Text = "Килент: " + _order.Customer.FullName;
            orderRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table purchasesTable = document.Tables.Add(tableRange, allPurchases.Count() + 1, 4);
            purchasesTable.Borders.InsideLineStyle = purchasesTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            purchasesTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;
            cellRange = purchasesTable.Cell(1, 1).Range;
            cellRange.Text = "Наименование";
            cellRange = purchasesTable.Cell(1, 2).Range;
            cellRange.Text = "цена";
            cellRange = purchasesTable.Cell(1, 3).Range;
            cellRange.Text = "Количество";
            cellRange = purchasesTable.Cell(1, 4).Range;
            cellRange.Text = "Сумма";

            purchasesTable.Rows[1].Range.Bold = 1;
            purchasesTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < allPurchases.Count(); i++)
            {
                Purchase purchase = allPurchases[i];

                cellRange = purchasesTable.Cell(i + 2, 1).Range;
                cellRange.Text = allPurchases[i].Product.ProductName;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                cellRange = purchasesTable.Cell(i + 2, 2).Range;
                cellRange.Text = allPurchases[i].Price + " руб";

                cellRange = purchasesTable.Cell(i + 2, 3).Range;
                cellRange.Text = allPurchases[i].Amount + " шт";

                cellRange = purchasesTable.Cell(i + 2, 4).Range;
                cellRange.Text = (allPurchases[i].Price * allPurchases[i].Amount) + " руб";
                cellRange.Font.Bold = 1;
            }
            Word.Paragraph totalParagraph = document.Paragraphs.Add();
            Word.Range totalRange = totalParagraph.Range;
            totalRange.Text = "Итого: " + _order.TotalPrice;
            totalRange.InsertParagraphAfter();

            if (MessageBox.Show("Файл будет сохранён на диске D\nПродолжить?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                document.SaveAs2(@"C:\Users\User\Desktop\order" + _order.Id + ".pdf", Word.WdExportFormat.wdExportFormatPDF);
        }

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _order.OrderStatus = ComboStatus.SelectedItem as OrderStatus;
        }
    }
}
