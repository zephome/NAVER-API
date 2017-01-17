using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Romanization.Models.Romanization;

namespace Romanization.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 한글인명-로마자 변환 (Naver API)
        /// Documant: https://developers.naver.com/docs/roman/overview
        /// </summary>
        /// <param name="query">검색어</param>
        /// <returns></returns>
        public ActionResult Romanization(string query)
        {
            // 결과 변수
            var errorCode = string.Empty;
            var errorMessage = string.Empty;
            var name = string.Empty;
            List<AItem> nameList = null;

            // API Host, Resource
            // https://openapi.naver.com/v1/krdict/romanization?query=홍길동
            var apiHost = "https://openapi.naver.com";
            var apiResource = "v1/krdict/romanization";

            // API URL Setting
            var client = new RestClient(apiHost);
            var request = new RestRequest(apiResource, Method.GET);
            request.AddParameter("query", query);

            // HTTP Headers Setting (네이버 API 에서 발급받은 키: https://developers.naver.com/docs/common/register)
            request.AddHeader("X-Naver-Client-Id", "2MrE8XpA9TXUIXdLFpCK");
            request.AddHeader("X-Naver-Client-Secret", "bk6pzFnMgF");

            // API Execute
            var response = client.Execute(request);

            try
            {
                // 응답코드가 성공일 경우 (Http Status Code: 200)
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // JSON Deserialize
                    var roman = JsonConvert.DeserializeObject<Roman>(response.Content);
                    var list = roman.aResult.Count;

                    // 로마자 변환 결과값이 존재
                    if (list > 0)
                    {
                        // 점수가 가장 높은 이름
                        name = roman.aResult[roman.aResult.Count - 1].aItems[0].name.ToString();
                        
                        // 이름 목록
                        nameList = roman.aResult[roman.aResult.Count - 1].aItems.ToList();
                    }
                    else
                    {
                        errorCode = "999";
                        errorMessage = "일치하는 데이터가 없습니다.";
                    }
                }
                else
                {
                    // JSON Deserialize
                    var romanError = JsonConvert.DeserializeObject<RomanizationError>(response.Content);

                    // 응답받은 에러코드, 내용
                    errorCode = romanError.errorCode;
                    errorMessage = romanError.errorMessage;
                }
            }
            catch (Exception ex)
            {
                errorCode = response.StatusCode.ToString();
                errorMessage = ex.Message.ToString();
            }

            // JSON Serialize
            //{
            //    "statusCode": 200,
            //    "errorMessage": "",
            //    "errorCode": "",
            //    "name": "Hong Gildong",
            //    "nameList": [
            //    {
            //        "name": "Hong Gildong",
            //        "score": "99"
            //    },
            //    {
            //        "name": "Hong Kildong",
            //        "score": "96"
            //    },
            //    {
            //        "name": "Hong Gildoung",
            //        "score": "21"
            //    },
            //    {
            //        "name": "Hong Kildoung",
            //        "score": "20"
            //    }
            //    ]
            //}
            RomanizationResult romanResult = new RomanizationResult()
            {
                statusCode = Convert.ToInt32(System.Net.HttpStatusCode.OK),
                errorCode = errorCode,
                errorMessage = errorMessage,
                name = name,
                nameList = nameList
            };

            return Json(romanResult, JsonRequestBehavior.AllowGet);
        }
    }
}