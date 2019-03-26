using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContractTester.Models;
using System.IO;
using System.Text;
using System.Net.Mime;
using ContractTester.Service;
using ContractTester.ViewModels;

namespace ContractTester
{
    public class ContractsController : Controller
    {
        private readonly ContractTesterContext _context;
        //private TesterService _testerService;

        public ContractsController(ContractTesterContext context)
        {
            _context = context;

        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contract.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        public async Task<FileStreamResult> Download(string id)
        {
            //Generate Error File
            string error = "Error no Contract found.";
            string downloadFileName = "Error.txt";

            if(id == null)
            {
                return null;
            }
            var contract = await _context.Contract.FirstOrDefaultAsync(x => x.Id == id);
            if(contract == null)
            {
                return null;
            }
            downloadFileName = $"{contract.Id}_{contract.VersionNumber}.txt";
            byte[] byteArray = Encoding.UTF8.GetBytes(contract.ContractString);

            var stream = new MemoryStream(byteArray);


            var fileStreamResult = new FileStreamResult(stream, MediaTypeNames.Text.Plain);
            fileStreamResult.FileDownloadName = downloadFileName;

            return fileStreamResult;

        }


        // GET: Contracts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,ContractString,VersionNumber,UpdateInst")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Description,ContractString,VersionNumber")] Contract contract)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                contract.UpdateInst = DateTime.Now;
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }


        //Get Contracts/TestMessage/1
        public async Task<IActionResult> TestMessage(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            TestMessageViewModel viewModel = new TestMessageViewModel()
            {
                contract = contract,
            };
            

            return View(viewModel);


        }

        [HttpPost]
        public async Task<IActionResult> TestMessage(string id, string sampleMessage)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            TesterService testerService = new TesterService(contract, sampleMessage);
            testerService.TestSetup();

            ViewModels.TestMessageViewModel viewModel = new TestMessageViewModel()
            {
                contract = contract,
                TestMessage = sampleMessage,
                TestMessageValid = testerService.IsMessageValid() ? "Valid" : "Invalid"
            };

                //return view which shows the message passed
            return View(viewModel);


        }


        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var contract = await _context.Contract.FindAsync(id);
            _context.Contract.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(string id)
        {
            return _context.Contract.Any(e => e.Id == id);
        }
    }
}
