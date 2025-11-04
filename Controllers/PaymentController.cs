using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using GuitarShop.DTOs;

namespace GuitarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly string vnp_TmnCode = "4AZ7IK8E";
        private readonly string vnp_HashSecret = "F1VPZ2SJSOFHKC2ZBEOHN0ZC5U8R1QX5";
        private readonly string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private readonly string vnp_ReturnUrl = "https://vaughn-easeled-yoshiko.ngrok-free.dev/api/payment/vnpay-return";

        [HttpPost("create-payment")]
        public IActionResult CreatePaymentUrl([FromForm] OrderRequest order, [FromForm] string language, [FromForm] string ordertype, [FromForm] string bankcode)
        {
            // 1. Tạo dictionary các tham số VNPay
            var vnp_Params = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Locale", string.IsNullOrEmpty(language) ? "vn" : language },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", string.IsNullOrEmpty(order.OrderId) ? DateTime.Now.Ticks.ToString() : order.OrderId },
                { "vnp_OrderInfo", order.OrderDescription ?? "Thanh toán đơn hàng" },
                { "vnp_OrderType", string.IsNullOrEmpty(ordertype) ? "other" : ordertype },
                { "vnp_Amount", ((long)order.Amount * 100).ToString() },
                { "vnp_ReturnUrl", vnp_ReturnUrl },
                { "vnp_IpAddr", GetIpAddress() },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };

            if (!string.IsNullOrEmpty(bankcode))
                vnp_Params.Add("vnp_BankCode", bankcode);

            // 2. Tạo chuỗi ký HMAC SHA512 (chưa encode)
            string signData = string.Join("&", vnp_Params
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .Select(kv => $"{kv.Key}={kv.Value}"));

            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);

            // 3. Encode query string gửi browser (encode key/value + khoảng trắng => +)
            string Encode(string value) => HttpUtility.UrlEncode(value).Replace("%20", "+");
            var query = string.Join("&", vnp_Params
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .Select(kv => $"{Encode(kv.Key)}={Encode(kv.Value)}"));
            query += $"&vnp_SecureHashType=HmacSHA512&vnp_SecureHash={vnp_SecureHash}";

            string paymentUrl = vnp_Url + "?" + query;

            return Ok(new { paymentUrl });
        }

        [HttpGet("vnpay-return")]
        public IActionResult PaymentReturn()
        {
            var queryData = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

            if (!queryData.TryGetValue("vnp_SecureHash", out var receivedHash))
                return BadRequest("Thiếu chữ ký");

            queryData.Remove("vnp_SecureHash");
            queryData.Remove("vnp_SecureHashType");

            // Sắp xếp lại và tạo hash để kiểm tra
            var sorted = new SortedDictionary<string, string>(queryData, StringComparer.Ordinal);
            string signData = string.Join("&", sorted.Select(kv => $"{kv.Key}={kv.Value}"));
            string calculatedHash = HmacSHA512(vnp_HashSecret, signData);

            if (!string.Equals(calculatedHash, receivedHash, StringComparison.OrdinalIgnoreCase))
                return BadRequest("Chữ ký không hợp lệ");

            return Ok(new
            {
                Message = "Thanh toán thành công (callback từ VNPay)!",
                Data = sorted
            });
        }

        private string HmacSHA512(string key, string inputData)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        }

        private string GetIpAddress()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ip) || ip.Contains(":"))
                ip = "127.0.0.1";
            return ip;
        }
    }
}
