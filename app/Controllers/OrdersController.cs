using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Controllers;

public class OrdersController : Controller
{
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var model = GetMockOrders();
        return View(model);
    }

    [HttpPost]
    public IActionResult OrderAgain(int orderId)
    {
        TempData["Notification"] = "Order has been placed again successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult ReturnItem(int orderId)
    {
        TempData["Notification"] = "Return request has been submitted.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult LeaveFeedback(int orderId, string feedback)
    {
        TempData["Notification"] = "Thank you for your feedback!";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult DismissNotification()
    {
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Generates mock order data grouped by date. Will be replaced with DB queries later.
    /// Groups are sorted by date descending (latest first).
    /// </summary>
    private static ViewOrdersViewModel GetMockOrders()
    {
        var orderGroups = new List<OrderGroupViewModel>
        {
            // ── Feb 8 ── 5 items
            new()
            {
                OrderId = 10,
                OrderNumber = "ORD-2026-0010",
                OrderDate = new DateTime(2026, 2, 8),
                Status = "Processing",
                StatusTimeframe = "5 Days",
                TotalPrice = 9350.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 1,
                        ProductName = "Lithium-6 Enriched",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Li-6+Enriched",
                        Description =
                            "Enriched Lithium-6 compound for thermonuclear research. Delivered in inert atmosphere packaging with chain-of-custody tracking.",
                        Price = 3250.00m,
                        Specifications =
                            "Element: Lithium (Li)\nIsotope: Li-6\nEnrichment: 95%+\nForm: Metallic ingot\nMass: 50g\nPackaging: Argon-sealed container",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                    new()
                    {
                        Id = 2,
                        ProductName = "Deuterium Gas Cylinder",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=D2+Gas",
                        Description =
                            "High-purity deuterium gas (D\u2082) in a DOT-approved aluminum cylinder for fusion research and spectroscopy applications.",
                        Price = 1250.00m,
                        Specifications =
                            "Element: Hydrogen (H)\nIsotope: Deuterium (D / H-2)\nPurity: 99.99%\nVolume: 50L cylinder\nPressure: 150 bar\nCertification: DOT 3AL",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                    new()
                    {
                        Id = 3,
                        ProductName = "Beryllium-7 Tracer",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Be-7+Tracer",
                        Description =
                            "Carrier-free Beryllium-7 radiotracer solution for environmental studies and atmospheric research.",
                        Price = 1800.00m,
                        Specifications =
                            "Element: Beryllium (Be)\nIsotope: Be-7\nActivity: 50 \u00b5Ci\nForm: Aqueous HCl solution\nHalf-life: 53.2 days\nCertification: NIST-traceable",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987. ISO 9001:2015 certified facility.",
                    },
                    new()
                    {
                        Id = 4,
                        ProductName = "Helium-3 Gas Ampoule",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=He-3+Gas",
                        Description =
                            "Ultra-pure Helium-3 gas for neutron detection, cryogenic research, and magnetic resonance imaging contrast.",
                        Price = 2100.00m,
                        Specifications =
                            "Element: Helium (He)\nIsotope: He-3\nPurity: 99.999%\nVolume: 1 liter STP\nForm: Sealed glass ampoule\nSource: Tritium decay",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 5,
                        ProductName = "Boron-10 Enriched Powder",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=B-10+Powder",
                        Description =
                            "Enriched Boron-10 powder for neutron shielding, BNCT cancer therapy research, and nuclear reactor control applications.",
                        Price = 950.00m,
                        Specifications =
                            "Element: Boron (B)\nIsotope: B-10\nEnrichment: 96%\nForm: Amorphous powder\nMass: 100g\nPackaging: Vacuum-sealed pouch",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                },
            },
            // ── Feb 5 ── 3 items
            new()
            {
                OrderId = 9,
                OrderNumber = "ORD-2026-0009",
                OrderDate = new DateTime(2026, 2, 5),
                Status = "Shipped",
                StatusTimeframe = "2 Days",
                TotalPrice = 17649.98m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 6,
                        ProductName = "Uranium-235 Sample",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=U-235+Sample",
                        Description =
                            "High-purity Uranium-235 isotope sample for research applications. Sealed in a certified containment vessel.",
                        Price = 12499.99m,
                        Specifications =
                            "Element: Uranium (U)\nIsotope: U-235\nPurity: 99.7%\nMass: 5g\nContainment: Type-B certified vessel\nRegulatory: NRC licensed, IAEA compliant",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987. ISO 9001:2015 certified facility.",
                    },
                    new()
                    {
                        Id = 7,
                        ProductName = "Plutonium-238 RTG Pellet",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Pu-238+RTG",
                        Description =
                            "Plutonium-238 oxide pellet designed for radioisotope thermoelectric generators. Flight-heritage qualified.",
                        Price = 4500.00m,
                        Specifications =
                            "Element: Plutonium (Pu)\nIsotope: Pu-238\nForm: Ceramic oxide pellet\nThermal output: 0.54 W/g\nEncapsulation: Iridium alloy clad\nCertification: NASA flight-qualified",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987. ISO 9001:2015 certified facility.",
                    },
                    new()
                    {
                        Id = 8,
                        ProductName = "Tritium Vial (10 Ci)",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Tritium+Vial",
                        Description =
                            "Sealed glass vial containing 10 Ci of gaseous tritium for self-luminous device manufacturing and tracer studies.",
                        Price = 649.99m,
                        Specifications =
                            "Element: Hydrogen (H)\nIsotope: Tritium (H-3)\nActivity: 10 Ci\nForm: Gas in sealed borosilicate vial\nHalf-life: 12.3 years\nRegulatory: NRC exempt quantity",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                },
            },
            // ── Jan 31 ── 2 items
            new()
            {
                OrderId = 8,
                OrderNumber = "ORD-2026-0008",
                OrderDate = new DateTime(2026, 1, 31),
                Status = "Delivered",
                StatusTimeframe = "Completed",
                TotalPrice = 5400.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 9,
                        ProductName = "Xenon-133 Gas Vial",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Xe-133+Gas",
                        Description =
                            "Medical-grade Xenon-133 gas for pulmonary ventilation imaging studies in nuclear medicine departments.",
                        Price = 3200.00m,
                        Specifications =
                            "Element: Xenon (Xe)\nIsotope: Xe-133\nActivity: 10 mCi\nForm: Gas in sealed vial\nHalf-life: 5.2 days\nCertification: USP compliant",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 10,
                        ProductName = "Krypton-85 Source",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Kr-85+Source",
                        Description =
                            "Sealed Krypton-85 beta source for spark gap triggers and leak detection in hermetic electronic packages.",
                        Price = 2200.00m,
                        Specifications =
                            "Element: Krypton (Kr)\nIsotope: Kr-85\nActivity: 100 mCi\nForm: Sealed gas capsule\nHalf-life: 10.76 years\nEncapsulation: Welded titanium",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                },
            },
            // ── Jan 28 ── 1 item
            new()
            {
                OrderId = 7,
                OrderNumber = "ORD-2026-0007",
                OrderDate = new DateTime(2026, 1, 28),
                Status = "Delivered",
                StatusTimeframe = "Completed",
                TotalPrice = 2100.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 11,
                        ProductName = "Strontium-90 Sealed Source",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Sr-90+Source",
                        Description =
                            "Calibrated Strontium-90 beta source for thickness gauges and academic demonstration.",
                        Price = 2100.00m,
                        Specifications =
                            "Element: Strontium (Sr)\nIsotope: Sr-90\nActivity: 5 mCi\nForm: Sealed ceramic disc\nEncapsulation: Double stainless steel\nCertification: ISO 2919 C43515",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                },
            },
            // ── Jan 22 ── BIG ORDER: 15 items (stress test)
            new()
            {
                OrderId = 6,
                OrderNumber = "ORD-2026-0006",
                OrderDate = new DateTime(2026, 1, 22),
                Status = "Delivered",
                StatusTimeframe = "Completed",
                TotalPrice = 68750.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 20,
                        ProductName = "Cobalt-60 Sealed Source",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Co-60+Source",
                        Description =
                            "Calibrated Cobalt-60 sealed source for industrial radiography and medical equipment calibration.",
                        Price = 8750.00m,
                        Specifications =
                            "Element: Cobalt (Co)\nIsotope: Co-60\nActivity: 50 Ci\nForm: Sealed source\nEncapsulation: Double-walled stainless steel",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 21,
                        ProductName = "Cesium-137 Capsule",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Cs-137+Cap",
                        Description =
                            "Cesium-137 sealed capsule for well-logging equipment calibration. Meets all transport regulations.",
                        Price = 5200.00m,
                        Specifications =
                            "Element: Cesium (Cs)\nIsotope: Cs-137\nActivity: 100 mCi\nForm: Pressed ceramic pellet\nHalf-life: 30.2 years",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                    new()
                    {
                        Id = 22,
                        ProductName = "Iridium-192 Wire",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Ir-192+Wire",
                        Description =
                            "Iridium-192 sealed wire source for gamma radiography of pipelines and structural welds.",
                        Price = 3400.00m,
                        Specifications =
                            "Element: Iridium (Ir)\nIsotope: Ir-192\nActivity: 80 Ci\nForm: Wire, 1mm diameter\nLength: 10 cm active",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 23,
                        ProductName = "Americium-241 Disc",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Am-241+Disc",
                        Description =
                            "Americium-241 alpha source disc for smoke detector manufacturing and XRF analyzer calibration.",
                        Price = 2500.00m,
                        Specifications =
                            "Element: Americium (Am)\nIsotope: Am-241\nActivity: 1 \u00b5Ci\nForm: Electrodeposited on steel disc\nDiameter: 25mm",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                    new()
                    {
                        Id = 24,
                        ProductName = "Californium-252 Neutron Source",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Cf-252+Src",
                        Description =
                            "Californium-252 neutron source for reactor start-up, prompt gamma neutron activation analysis, and borehole logging.",
                        Price = 12000.00m,
                        Specifications =
                            "Element: Californium (Cf)\nIsotope: Cf-252\nActivity: 5 \u00b5g\nNeutron output: 1.16\u00d710\u2077 n/s\nHalf-life: 2.645 years",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                    new()
                    {
                        Id = 25,
                        ProductName = "Phosphorus-32 Solution",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=P-32+Soln",
                        Description =
                            "Carrier-free Phosphorus-32 in aqueous solution for molecular biology labelling and autoradiography.",
                        Price = 450.00m,
                        Specifications =
                            "Element: Phosphorus (P)\nIsotope: P-32\nActivity: 1 mCi\nForm: Aqueous orthophosphate\nHalf-life: 14.3 days",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 26,
                        ProductName = "Sulphur-35 Methionine",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=S-35+Met",
                        Description =
                            "L-[35S]-Methionine for protein biosynthesis labelling in cell culture and in vivo metabolic studies.",
                        Price = 680.00m,
                        Specifications =
                            "Element: Sulphur (S)\nIsotope: S-35\nActivity: 250 \u00b5Ci\nSpecific activity: >1000 Ci/mmol\nHalf-life: 87.5 days",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                    new()
                    {
                        Id = 27,
                        ProductName = "Carbon-14 Glucose",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=C-14+Glc",
                        Description =
                            "D-[U-14C]-Glucose uniformly labelled for metabolic pathway tracing and pharmacokinetic ADME studies.",
                        Price = 920.00m,
                        Specifications =
                            "Element: Carbon (C)\nIsotope: C-14\nActivity: 50 \u00b5Ci\nSpecific activity: 300 mCi/mmol\nHalf-life: 5,730 years",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                    new()
                    {
                        Id = 28,
                        ProductName = "Iodine-125 Seeds (10-pack)",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=I-125+Seeds",
                        Description =
                            "Iodine-125 brachytherapy seeds for low-dose-rate permanent implant treatment of prostate and ocular cancers.",
                        Price = 7500.00m,
                        Specifications =
                            "Element: Iodine (I)\nIsotope: I-125\nActivity: 0.5 mCi/seed\nForm: Titanium-encapsulated seeds\nQuantity: 10 seeds\nHalf-life: 59.4 days",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 29,
                        ProductName = "Thallium-201 Chloride",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Tl-201+Cl",
                        Description =
                            "Thallium-201 chloride injection for myocardial perfusion imaging (cardiac stress testing).",
                        Price = 3600.00m,
                        Specifications =
                            "Element: Thallium (Tl)\nIsotope: Tl-201\nActivity: 4 mCi\nForm: Sterile injectable solution\nHalf-life: 72.9 hours",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                    new()
                    {
                        Id = 30,
                        ProductName = "Gallium-68 Generator",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Ga-68+Gen",
                        Description =
                            "Germanium-68/Gallium-68 generator for on-demand PET radiopharmaceutical preparation.",
                        Price = 9800.00m,
                        Specifications =
                            "Element: Gallium (Ga)\nIsotope: Ga-68\nParent: Ge-68\nCalibrated activity: 50 mCi\nShelf life: 12 months\nElution: 0.1M HCl",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 31,
                        ProductName = "Lutetium-177 Chloride",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Lu-177+Cl",
                        Description =
                            "No-carrier-added Lutetium-177 in chloride form for DOTATATE and PSMA radioligand therapy.",
                        Price = 4200.00m,
                        Specifications =
                            "Element: Lutetium (Lu)\nIsotope: Lu-177\nActivity: 250 mCi\nForm: n.c.a. in 0.04M HCl\nSpecific activity: >3000 GBq/mg\nHalf-life: 6.64 days",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                    new()
                    {
                        Id = 32,
                        ProductName = "Yttrium-90 Microspheres",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Y-90+Sph",
                        Description =
                            "Yttrium-90 resin microspheres for selective internal radiation therapy (SIRT) of hepatic malignancies.",
                        Price = 5500.00m,
                        Specifications =
                            "Element: Yttrium (Y)\nIsotope: Y-90\nActivity: 3 GBq\nForm: Resin microspheres 20\u201360\u00b5m\nHalf-life: 64.1 hours",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 33,
                        ProductName = "Samarium-153 EDTMP",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Sm-153",
                        Description =
                            "Samarium-153 lexidronam (EDTMP) for palliative treatment of painful osteoblastic bone metastases.",
                        Price = 2850.00m,
                        Specifications =
                            "Element: Samarium (Sm)\nIsotope: Sm-153\nActivity: 1 mCi/kg dose\nForm: Sterile injectable complex\nHalf-life: 46.3 hours",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                    new()
                    {
                        Id = 34,
                        ProductName = "Rhenium-186 Sulfide Colloid",
                        ProductImage =
                            "https://placehold.co/400x300/e5e5e5/737373?text=Re-186+Coll",
                        Description =
                            "Rhenium-186 sulfide colloid for radiosynovectomy treatment of inflammatory joint disease.",
                        Price = 1400.00m,
                        Specifications =
                            "Element: Rhenium (Re)\nIsotope: Re-186\nActivity: 10 mCi\nForm: Colloidal suspension\nParticle size: 2\u20135 \u00b5m\nHalf-life: 3.72 days",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                },
            },
            // ── Jan 20 ── 4 items
            new()
            {
                OrderId = 5,
                OrderNumber = "ORD-2026-0005",
                OrderDate = new DateTime(2026, 1, 20),
                Status = "Delivered",
                StatusTimeframe = "Completed",
                TotalPrice = 19850.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 40,
                        ProductName = "Cobalt-60 Source (Cal)",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Co-60+Cal",
                        Description =
                            "Cobalt-60 calibration source for radiation therapy machine QA and dosimetry.",
                        Price = 8750.00m,
                        Specifications =
                            "Element: Cobalt (Co)\nIsotope: Co-60\nActivity: 50 Ci\nForm: Sealed source\nUse: Calibration",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 41,
                        ProductName = "Cesium-137 Capsule (QA)",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Cs-137+QA",
                        Description =
                            "Cesium-137 reference capsule for survey meter calibration and emergency response training.",
                        Price = 5200.00m,
                        Specifications =
                            "Element: Cesium (Cs)\nIsotope: Cs-137\nActivity: 100 mCi\nForm: Ceramic pellet\nUse: QA & training",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                    new()
                    {
                        Id = 42,
                        ProductName = "Iridium-192 Wire (NDT)",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Ir-192+NDT",
                        Description =
                            "Iridium-192 gamma source for non-destructive testing of pipeline welds and structural steel.",
                        Price = 3400.00m,
                        Specifications =
                            "Element: Iridium (Ir)\nIsotope: Ir-192\nActivity: 80 Ci\nForm: Sealed wire\nUse: Radiography NDT",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 43,
                        ProductName = "Americium-241 Disc (XRF)",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Am-241+XRF",
                        Description =
                            "Americium-241 disc source for portable XRF mineral analysis and alloy identification.",
                        Price = 2500.00m,
                        Specifications =
                            "Element: Americium (Am)\nIsotope: Am-241\nActivity: 1 \u00b5Ci\nForm: Electrodeposited disc\nUse: XRF analysis",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                },
            },
            // ── Jan 15 ── 2 items
            new()
            {
                OrderId = 4,
                OrderNumber = "ORD-2026-0004",
                OrderDate = new DateTime(2026, 1, 15),
                Status = "Delivered",
                StatusTimeframe = "Completed",
                TotalPrice = 4100.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 50,
                        ProductName = "Zinc-65 Chloride",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Zn-65+Cl",
                        Description =
                            "Zinc-65 chloride solution for zinc metabolism studies and environmental tracer research.",
                        Price = 1600.00m,
                        Specifications =
                            "Element: Zinc (Zn)\nIsotope: Zn-65\nActivity: 100 \u00b5Ci\nForm: Aqueous ZnCl\u2082\nHalf-life: 243.9 days",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                    new()
                    {
                        Id = 51,
                        ProductName = "Iron-59 Citrate",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Fe-59+Cit",
                        Description =
                            "Iron-59 citrate complex for ferrokinetics studies and iron absorption measurement in clinical trials.",
                        Price = 2500.00m,
                        Specifications =
                            "Element: Iron (Fe)\nIsotope: Fe-59\nActivity: 50 \u00b5Ci\nForm: Ferric citrate complex\nHalf-life: 44.5 days",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                },
            },
            // ── Jan 10 ── 2 items
            new()
            {
                OrderId = 3,
                OrderNumber = "ORD-2026-0003",
                OrderDate = new DateTime(2026, 1, 10),
                Status = "Delivered",
                StatusTimeframe = "Completed",
                TotalPrice = 6150.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 60,
                        ProductName = "Technetium-99m Generator",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Tc-99m+Gen",
                        Description =
                            "Mo-99/Tc-99m generator for hospital nuclear medicine departments. Weekly shipment program.",
                        Price = 4650.00m,
                        Specifications =
                            "Parent: Mo-99\nDaughter: Tc-99m\nCalibration: 15 GBq\nShelf life: 7 days\nElution: 5\u201320 mL",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                    new()
                    {
                        Id = 61,
                        ProductName = "Radium-226 Needle",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Ra-226+Ndl",
                        Description =
                            "Legacy Radium-226 brachytherapy needle for museum display or educational demonstration.",
                        Price = 1500.00m,
                        Specifications =
                            "Element: Radium (Ra)\nIsotope: Ra-226\nActivity: 1 mg Ra-eq\nForm: Sealed needle\nLength: 4 cm",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                },
            },
            // ── Jan 3 ── 3 items
            new()
            {
                OrderId = 2,
                OrderNumber = "ORD-2026-0002",
                OrderDate = new DateTime(2026, 1, 3),
                Status = "Delivered",
                StatusTimeframe = "Completed",
                TotalPrice = 7800.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 70,
                        ProductName = "Sodium-22 Point Source",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Na-22+Pt",
                        Description =
                            "Sodium-22 positron point source for PET scanner calibration and positron annihilation spectroscopy.",
                        Price = 2800.00m,
                        Specifications =
                            "Element: Sodium (Na)\nIsotope: Na-22\nActivity: 10 \u00b5Ci\nForm: Sealed disc source\nHalf-life: 2.6 years",
                        Manufacturer = "NucleoTech Solutions",
                        ManufacturerDescription =
                            "Global provider of sealed radioactive sources for medical, industrial, and research applications.",
                    },
                    new()
                    {
                        Id = 71,
                        ProductName = "Manganese-54 Standard",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Mn-54+Std",
                        Description =
                            "Manganese-54 gamma-ray energy calibration standard for high-purity germanium detector efficiency curves.",
                        Price = 1800.00m,
                        Specifications =
                            "Element: Manganese (Mn)\nIsotope: Mn-54\nActivity: 1 \u00b5Ci\nForm: Sealed point source\nHalf-life: 312.2 days",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                    new()
                    {
                        Id = 72,
                        ProductName = "Barium-133 Check Source",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Ba-133+Chk",
                        Description =
                            "Barium-133 check source for daily constancy verification of gamma cameras and survey meters.",
                        Price = 3200.00m,
                        Specifications =
                            "Element: Barium (Ba)\nIsotope: Ba-133\nActivity: 5 \u00b5Ci\nForm: Epoxy-encapsulated disc\nHalf-life: 10.5 years",
                        Manufacturer = "Cascade Isotopes Inc.",
                        ManufacturerDescription =
                            "Specializing in light-element isotope enrichment. DOE-approved vendor with 30+ years in the field.",
                    },
                },
            },
            // ── Dec 20 ── 1 item
            new()
            {
                OrderId = 1,
                OrderNumber = "ORD-2025-0001",
                OrderDate = new DateTime(2025, 12, 20),
                Status = "Delivered",
                StatusTimeframe = "Completed",
                TotalPrice = 15000.00m,
                Items = new List<OrderItemViewModel>
                {
                    new()
                    {
                        Id = 80,
                        ProductName = "Curium-244 Alpha Source",
                        ProductImage = "https://placehold.co/400x300/e5e5e5/737373?text=Cm-244+Src",
                        Description =
                            "Curium-244 alpha-particle source for smoke detector type-testing and alpha spectrometer calibration.",
                        Price = 15000.00m,
                        Specifications =
                            "Element: Curium (Cm)\nIsotope: Cm-244\nActivity: 5 \u00b5Ci\nForm: Electrodeposited on Pt disc\nHalf-life: 18.1 years",
                        Manufacturer = "Prometheus Atomics Labs",
                        ManufacturerDescription =
                            "Leading supplier of rare isotopes and nuclear research materials since 1987.",
                    },
                },
            },
        };

        return new ViewOrdersViewModel
        {
            OrderGroups = orderGroups,
            UserName = "Dr. Elena Vasquez",
            UserRole = "Senior Procurement Specialist",
            UserAvatarInitials = "EV",
            NotificationMessage =
                "Your Uranium-235 order is out for delivery and will arrive within 2 business days.",
        };
    }
}
