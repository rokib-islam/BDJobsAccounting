using AccountingSystem.Abstractions.BLL;
using AccountingSystem.BLL;
using AccountingSystem.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountingSystem.Web.Controllers
{
    public class LedgerController : Controller
    {
        private readonly ILedgerManager _LedgerManager;
        private readonly IJournalManager _JournalManager;

        public LedgerController(ILedgerManager LedgerManager, IJournalManager journalManager)
        {
            _LedgerManager = LedgerManager;
            _JournalManager = journalManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetService(int sTypy)
        {
            var result = await _LedgerManager.GetService(sTypy);

            return Json(result);
        }
        public async Task<IActionResult> GetAllLedger()
        {
            var isAccount = HttpContext.Session.GetInt32("AccountDep").ToString();
            var isAdmin = HttpContext.Session.GetInt32("CanModifyAdmin").ToString();

            var result = await _LedgerManager.GetAllLedger(isAdmin, isAccount);


            return Json(result);
        }

        public async Task<IActionResult> GetAllEveryLedger(string isCashCollection)
        {
            var result = await _LedgerManager.GetAllEveryLedger(isCashCollection);
            return Json(result);
        }
        public async Task<IActionResult> GetOnlineLedgerId(string onlineProduct)
        {
            var data = await _LedgerManager.GetOnlineLedgerId(onlineProduct);

            return Json(data);
        }
        public async Task<IActionResult> GetProducts(int admin, int account, string groupname, string isAll, string isI, int isVatType)
        {
            var data = await _LedgerManager.GetProducts(admin, account, groupname, isAll, isI, isVatType);

            return Json(data);
        }
        public async Task<IActionResult> GetLedgersWithBalance()
        {
            var data = await _LedgerManager.GetLedgersWithBalance();

            return Json(data);
        }
        public async Task<IActionResult> GetSubgroups(string mainGroup)
        {
            var data = await _LedgerManager.GetAllLedgers();

            data = data
                .Where(x => x.MaingroupName == mainGroup && !x.LedgerAcc)
                .OrderBy(x => x.GroupName)
                .ToList();

            return Json(data);
        }
        public async Task<IActionResult> GetGroup(int groupId)
        {
            var data = await _LedgerManager.GetAllLedgers();

            var group = data.FirstOrDefault(x => x.Id == groupId);

            return Json(group);
        }
        public async Task<IActionResult> GetGroupByName(string name)
        {
            var data = await _LedgerManager.GetAllLedgers();

            var group = data.OrderBy(x => x.GroupName).FirstOrDefault(x => x.GroupName.ToLower().StartsWith(name.ToLower()));

            return Json(group);
        }
        public async Task<IActionResult> GetSubGroupsWithLedger(int groupId)
        {
            var data = await _LedgerManager.GetAllLedgers();

            var group = data.OrderBy(x => x.GroupName);

            return Json(group);
        }

        public async Task<IActionResult> Save(string group, string mainGroup, int? subGroupId, bool isLedger, int ServiceId)
        {

            var allLedger = await _LedgerManager.GetAllLedgers();
            var ledger = allLedger.FirstOrDefault(x => x.Id == subGroupId);
            var aLedger = new LedgerViewModel
            {
                GroupName = group,
                Under = ledger != null ? ledger.Under + "," + subGroupId : subGroupId.ToString(),
                MaingroupName = mainGroup,
                LevelNo = ledger != null ? ledger.LevelNo + 1 : 1,
                LedgerAcc = isLedger,
                ServiceID = ServiceId
            };

            await _LedgerManager.SaveLedgerAsync(aLedger);

            allLedger.Add(aLedger);

            var filteredLedgers = allLedger.Where(x => x.MaingroupName == mainGroup && !x.LedgerAcc)
                                            .OrderBy(x => x.GroupName)
                                            .ToList();
            return Json(filteredLedgers);
        }
        public async Task<JsonResult> Update(string group, string mainGroup, int groupId, int underId, bool isLedger, int ServiceId)
        {

            var allLedger = await _LedgerManager.GetAllLedgers();
            var selectedGroup = allLedger.FirstOrDefault(x => x.Id == groupId);
            selectedGroup.GroupName = group;
            var ledger = allLedger.FirstOrDefault(x => x.Id == underId);
            selectedGroup.Under = ledger.Under + "," + ledger.Id;
            if (ledger.Under == "0")
            {
                selectedGroup.Under = ledger.Id.ToString();
            }
            selectedGroup.MaingroupName = mainGroup;
            selectedGroup.LevelNo = ledger.LevelNo + 1;
            selectedGroup.LedgerAcc = isLedger;
            selectedGroup.ServiceID = ServiceId;

            await _LedgerManager.UpdateLedgerAsync(selectedGroup);

            return Json(true);
        }
        public async Task<IActionResult> Delete(int groupId, string gName)
        {
            try
            {
                var journalExists = await _JournalManager.GetJournalBySIdAsync(groupId);
                if (journalExists != null)
                {
                    return Conflict("You cannot delete this ledger. One or more journals exist for this selected ledger.");
                }

                var ledgers = await _LedgerManager.GetAllLedgers();
                var ledgersExist = ledgers.Any(x => x.Under.Contains(groupId.ToString()));

                if (ledgersExist)
                {
                    return Conflict("This group has one or more ledgers. You must delete those ledgers before deleting this group.");
                }

                var response = await _LedgerManager.DeleteLedgerAsync(groupId);
                return Json(response);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, $"An error occurred while deleting the ledger: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the ledger: {ex.Message}");
            }
        }

        //public async Task<IActionResult> GetProductListByKey(string startingKey)
        //{
        //    var result = await _LedgerManager.GetProductListByKey(startingKey);
        //    return Json(result);
        //}

        public async Task<IActionResult> GetProductList()
        {
            var result = await _LedgerManager.GetProductList();
            return Json(result);
        }

        //public async Task<IActionResult> GetProductById(int pId)
        //{
        //    var resp = await _LedgerManager.GetProductById(pId);
        //    return Json(resp);
        //}

        public IActionResult ListOfServices()
        {
            ClaimsPrincipal claimusers = HttpContext.User;
            if (claimusers.Identity.IsAuthenticated)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LoadServiceList([FromBody] LoadServiceListModel model)
        {
            var result = await _LedgerManager.LoadServiceList(model);
            return Json(result);
        }

        public async Task<IActionResult> GetStaffPFIAccountList()
        {
            var result = await _LedgerManager.GetStaffPFIAccountList();
            return Json(result);
        }
        
        public async Task<IActionResult> GetLedgerName()
        {
            var result = await _LedgerManager.GetLedgerName();
            return Json(result);
        }
    }


}

