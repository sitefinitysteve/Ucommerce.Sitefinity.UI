﻿using System;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Ucommerce.Sitefinity.UI.Mvc.Model.Interfaces;
using UCommerce.Infrastructure;
using UCommerce.Transactions;

namespace Ucommerce.Sitefinity.UI.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "uMiniBasket_MVC", Title = "Mini Basket", SectionName = UcommerceUIModule.UCOMMERCE_WIDGET_SECTION, ModuleName = UcommerceUIModule.NAME, CssClass = "sfMvcIcn")]
    public class MiniBasketController : Controller
    {
        public Guid? CartPageId { get; set; }
        public string TemplateName { get; set; } = "Index";
        private readonly TransactionLibraryInternal _transactionLibraryInternal;
        private readonly IMiniBasketService _miniBasketService;

        public MiniBasketController(IMiniBasketService miniBasketService)
        {
            _transactionLibraryInternal = ObjectFactory.Instance.Resolve<TransactionLibraryInternal>();
            _miniBasketService = miniBasketService;
        }

        public ActionResult Index()
        {
            var miniBasketModel = ResolveModel();
            var miniBasketRenderingViewModel = miniBasketModel.CreateViewModel(Url.Action("Refresh"));

            if (!_transactionLibraryInternal.HasBasket())
            {
                return View(TemplateName, miniBasketRenderingViewModel);
            }

            return View(TemplateName, miniBasketRenderingViewModel);
        }

        [HttpGet]
        public ActionResult Refresh()
        {
            return Json(_miniBasketService.Refresh(), JsonRequestBehavior.AllowGet);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, TemplateName);
        }

        private IMiniBasketModel ResolveModel()
        {
            var container = UcommerceUIModule.Container;
            var model = container.Resolve<IMiniBasketModel>(
                new
                {
                    cartPageId = this.CartPageId
                });

            return model;
        }
    }
}