using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Data.Seeders;

public class ProductSeeder(DbContext _context, List<Supplier> _availableSuppliers)
    : BaseSeeder<Product>(_context)
{
    private readonly List<Supplier> suppliers = _availableSuppliers;

    private static readonly List<(
        string Name,
        string ShortName,
        string Description,
        double UnitPrice,
        int Stock,
        int AtomicNumber,
        string StateOfMatter,
        string ProductType,
        string ProductSubtype,
        string HalfLife,
        string? Purity,
        string? SpecificActivity
    )> IsotopeProducts =
    [
        // Medical – diagnostic imaging
        (
            "Technetium-99m Generator",
            "Tc-99m Gen",
            "Mo-99/Tc-99m column generator for diagnostic SPECT imaging of cardiac, bone, renal and cerebral perfusion.",
            8500.00,
            20,
            43, // Tc
            "Solid",
            "Medical",
            "Diagnostic",
            "6 h",
            "≥99.9 %",
            null
        ),
        (
            "Iodine-131 Sodium Solution",
            "I-131 NaI",
            "Beta/gamma emitter supplied as sodium iodide solution for thyroid cancer ablation and hyperthyroidism treatment.",
            450.00,
            40,
            53, // I
            "Liquid",
            "Medical",
            "Therapeutic",
            "8.02 days",
            null,
            "≥185 GBq/mg"
        ),
        (
            "Iodine-123 Capsules",
            "I-123 Cap",
            "High-purity gamma emitter (159 keV) for thyroid function testing and diagnostic scintigraphy. Supplied as sodium iodide capsules.",
            320.00,
            50,
            53, // I
            "Solid",
            "Medical",
            "Diagnostic",
            "13.2 h",
            null,
            null
        ),
        (
            "Fluorine-18 FDG Injection",
            "F-18 FDG",
            "2-[¹⁸F]Fluoro-2-deoxy-D-glucose positron emitter for PET/CT imaging of oncology, neurology and cardiology.",
            1200.00,
            30,
            9, // F
            "Liquid",
            "Medical",
            "Diagnostic",
            "110 min",
            "≥95 % (radiochemical)",
            null
        ),
        (
            "Gallium-68 DOTATATE",
            "Ga-68 DOTA",
            "Somatostatin receptor PET tracer for staging and restaging of neuroendocrine tumours (NETs). Produced via Ge-68/Ga-68 generator.",
            2100.00,
            25,
            31, // Ga
            "Liquid",
            "Medical",
            "Diagnostic",
            "68 min",
            null,
            null
        ),
        (
            "Lutetium-177 DOTATATE",
            "Lu-177 DOTA",
            "Targeted radionuclide therapy (PRRT) for somatostatin receptor-positive NETs. Beta emitter with low-energy gamma for imaging. GMP grade.",
            45000.00,
            8,
            71, // Lu
            "Liquid",
            "Medical",
            "Therapeutic",
            "6.65 days",
            null,
            null
        ),
        (
            "Yttrium-90 SIR-Spheres Kit",
            "Y-90 SIR",
            "Resin microsphere radioembolisation kit for hepatocellular carcinoma and colorectal liver metastases. Pure beta emitter.",
            25000.00,
            12,
            39, // Y
            "Solid",
            "Medical",
            "Therapeutic",
            "64 h",
            null,
            null
        ),
        (
            "Radium-223 Dichloride Injection",
            "Ra-223 DCl",
            "Alpha emitter for the treatment of castration-resistant prostate cancer with symptomatic bone metastases.",
            55000.00,
            6,
            88, // Ra
            "Liquid",
            "Medical",
            "Therapeutic",
            "11.4 days",
            null,
            null
        ),
        (
            "Indium-111 Capromab Pendetide",
            "In-111 Cap",
            "Radioimmunoscintigraphy agent targeting PSMA for staging recurrent prostate cancer. Gamma emitter at 171 and 245 keV.",
            3400.00,
            18,
            49, // In
            "Liquid",
            "Medical",
            "Diagnostic",
            "2.8 days",
            null,
            null
        ),
        (
            "Thallium-201 Chloride Injection",
            "Tl-201 Cl",
            "Cardiac perfusion SPECT agent and parathyroid/thyroid scintigraphy tracer. Gamma/X-ray emitter.",
            1800.00,
            35,
            81, // Tl
            "Liquid",
            "Medical",
            "Diagnostic",
            "73 h",
            null,
            null
        ),
        // Industrial / Mining
        (
            "Cobalt-60 Industrial Gamma Source",
            "Co-60 Src",
            "High-activity gamma source (1.17 and 1.33 MeV) for radiographic inspection of welds, castings and pipelines. Encapsulated to ISO 2919.",
            3500.00,
            22,
            27, // Co
            "Solid",
            "Industrial",
            "NDT",
            "5.27 years",
            null,
            null
        ),
        (
            "Iridium-192 Radiography Source",
            "Ir-192 Src",
            "Compact gamma source for industrial non-destructive testing of steel structures and pipeline girth welds. Average energy 380 keV.",
            2800.00,
            30,
            77, // Ir
            "Solid",
            "Industrial",
            "NDT",
            "73.8 days",
            null,
            null
        ),
        (
            "Cesium-137 Density Gauge Source",
            "Cs-137 Gauge",
            "Single 662 keV gamma source for continuous level, density and moisture measurement in process industries and mining.",
            1500.00,
            45,
            55, // Cs
            "Solid",
            "Industrial",
            "Gauging",
            "30.2 years",
            null,
            null
        ),
        (
            "Americium-241 Calibration Source",
            "Am-241 Cal",
            "Alpha/low-energy gamma (60 keV) source for ionisation smoke detector calibration, moisture gauging and XRF analysers.",
            950.00,
            60,
            95, // Am
            "Solid",
            "Industrial",
            "Calibration",
            "432 years",
            null,
            null
        ),
        (
            "Californium-252 Neutron Source",
            "Cf-252 Src",
            "Spontaneous-fission neutron source for oil and mineral well logging, on-line bulk material analysis and reactor start-up.",
            60000.00,
            5,
            98, // Cf
            "Solid",
            "Industrial",
            "Neutron Source",
            "2.65 years",
            null,
            null
        ),
        (
            "Selenium-75 Pipeline Weld Source",
            "Se-75 Src",
            "Low-energy gamma source (66–401 keV) for radiographic inspection of thin-wall pipe welds and small-bore fittings.",
            1800.00,
            28,
            34, // Se
            "Solid",
            "Industrial",
            "NDT",
            "119.8 days",
            null,
            null
        ),
        // Research
        (
            "Carbon-14 Standard Solution",
            "C-14 Std",
            "NIST-traceable ¹⁴C reference standard for radiocarbon dating calibration and metabolic tracer studies. Supplied as benzene or oxalic acid matrix.",
            280.00,
            100,
            6, // C
            "Liquid",
            "Research",
            "Standard",
            "5,730 years",
            null,
            null
        ),
        (
            "Tritium (H-3) Calibration Standard",
            "H-3 Cal",
            "Low-energy beta emitter for liquid scintillation counter calibration and hydrogen transport tracer research. Supplied as tritiated water.",
            150.00,
            80,
            1, // H
            "Gas",
            "Research",
            "Calibration",
            "12.3 years",
            null,
            null
        ),
        (
            "Phosphorus-32 Orthophosphate Solution",
            "P-32 OPO4",
            "High-energy beta emitter for DNA/RNA radiolabelling, kinase assays and autoradiography in molecular biology. Carrier-free.",
            85.00,
            70,
            15, // P
            "Liquid",
            "Research",
            "Tracer",
            "14.3 days",
            null,
            null
        ),
        (
            "Molybdenum-99 Bulk Generator Supply",
            "Mo-99 Bulk",
            "Fission-produced Mo-99 bulk material for hospital and commercial radiopharmacy Tc-99m generator production.",
            12000.00,
            10,
            42, // Mo
            "Solid",
            "Medical",
            "Generator Precursor",
            "66 h",
            null,
            "≥740 GBq/g at calibration time"
        ),
    ];

    public new List<Product> Generate()
    {
        var products = IsotopeProducts
            .Select(
                (p, i) =>
                    new Product
                    {
                        Name = p.Name,
                        ShortName = p.ShortName,
                        Description = p.Description,
                        UnitPrice = p.UnitPrice,
                        StockQuantity = p.Stock,
                        AtomicNumber = p.AtomicNumber,
                        StateOfMatter = p.StateOfMatter,
                        ProductType = p.ProductType,
                        ProductSubtype = p.ProductSubtype,
                        HalfLife = p.HalfLife,
                        Purity = p.Purity,
                        SpecificActivity = p.SpecificActivity,
                        FkSupplierId = suppliers[i % suppliers.Count].Id,
                    }
            )
            .ToList();

        foreach (var product in products)
        {
            context.Add(product);
        }

        return products;
    }
}
