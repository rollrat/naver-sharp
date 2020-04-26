// This source code is a part of NAVER Open API Wrapper.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NaverOpenAPI.NaverAPI.Shopping
{
    [Command("v1/datalab/shopping/category/device")]
    public class ShoppingCategoryTrendDevice : ICommand<ShoppingCategoryTrendDeviceResponse>
    {
        /// <summary>
        /// 조회 기간 시작 날짜(yyyy-mm-dd 형식). 2017년 8월 1일부터 조회할 수 있습니다.
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 조회 기간 종료 날짜(yyyy-mm-dd 형식)
        /// </summary>
        public string endDate { get; set; }

        /// <summary>
        /// 구간 단위
        /// - date: 일간
        /// - week: 주간
        /// - month: 월간
        /// </summary>
        public string timeUnit { get; set; }

        /// <summary>
        /// 네이버 쇼핑의 분야 코드. 네이버쇼핑에서 카테고리를 선택했을 때의 URL에 있는 cat_id 파라미터의 값으로 분야 코드를 확인할 수 있습니다.
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 기기. 검색 환경에 따른 조건입니다.
        /// - 설정 안 함: 모든 기기에서의 검색 클릭 추이
        /// - pc: PC에서의 검색 클릭 추이
        /// - mo: 모바일 기기에서의 검색 클릭 추이
        /// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string device { get; set; }

        /// <summary>
        /// 성별. 검색 사용자의 성별에 따른 조건입니다.
        /// - 설정 안 함: 모든 성별
        /// - m: 남성
        /// - f: 여성
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string gender { get; set; }

        /// <summary>
        /// 연령. 검색 사용자의 연령에 따른 조건입니다.
        /// - 설정 안 함: 모든 연령
        /// - 10: 10∼19세
        /// - 20: 20∼29세
        /// - 30: 30∼39세
        /// - 40: 40∼49세
        /// - 50: 50∼59세
        /// - 60: 60세 이상
        /// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] ages { get; set; }
    }

    [CommandResponse("v1/datalab/shopping/category/device")]
    public class ShoppingCategoryTrendDeviceResponse
    {
        /// <summary>
        /// 조회 기간 시작 날짜(yyyy-mm-dd 형식)
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 조회 기간 종료 날짜(yyyy-mm-dd 형식)
        /// </summary>
        public string endDate { get; set; }

        /// <summary>
        /// 구간 단위
        /// - date: 일간
        /// - week: 주간
        /// - month: 월간
        /// </summary>
        public string timeUnit { get; set; }

        public class Result
        {
            /// <summary>
            /// 쇼핑 분야 이름
            /// </summary>
            public string title { get; set; }

            /// <summary>
            /// 쇼핑 분야 코드
            /// </summary>
            public string category { get; set; }

            public class Data
            {
                /// <summary>
                /// 구간별 시작 날짜(yyyy-mm-dd 형식)
                /// </summary>
                public string period { get; set; }

                /// <summary>
                /// 구간별 클릭량의 상대적 비율. 구간별 결과에서 가장 큰 값을 100으로 설정한 상댓값입니다.
                /// </summary>
                public double ratio { get; set; }
            }

            public Data[] data { get; set; }
        }

        public Result[] results { get; set; }
    }
}
