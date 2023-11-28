using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


public class Product
{
    public string Name { get; set; }
    public double InterestRate { get; set; }
    public string State { get; set; }
    public int CreditScore { get; set; }
    public bool Disqualified { get; set; }
}

public class PricingRule
{
    public string Condition { get; set; }
    public string Action { get; set; }
    public double ActionValue { get; set; }
}

public class PricingRules
{
    private PricingRule[] rules;

    // Constructor that loads pricing rules from a JSON file
    public PricingRules(string jsonFilePath)
    {
        LoadRules(jsonFilePath);
    }

    // Load pricing rules from a JSON file
    private void LoadRules(string jsonFilePath)
    {
        try
        {
            // Read the JSON file and deserialize it into an array of PricingRule objects
            string json = File.ReadAllText(jsonFilePath);
            rules = JsonSerializer.Deserialize<PricingRule[]>(json);
        }
        catch (Exception ex)
        {
            // Handle exceptions if there's an error loading the rules
            Console.WriteLine($"Error loading pricing rules: {ex.Message}");
        }
    }

    // Apply pricing rules to a product
    public void ApplyRules(Product product)
    {
        // Check if rules are loaded
        if (rules == null)
        {
            Console.WriteLine("Error: Pricing rules not loaded.");
            return;
        }

        // Iterate through each pricing rule
        foreach (var rule in rules)
        {
            // Evaluate the condition and apply the action if true
            if (EvaluateCondition(rule.Condition, product))
            {
                ApplyAction(rule.Action, product, rule.ActionValue);
            }
        }
    }

    // Evaluate a condition for a product
    private bool EvaluateCondition(string condition, Product product)
    {
        // Implement logic to evaluate conditions
        switch (condition.ToLower())
        {
            case "florida":
                return product.State.ToLower() == "florida";
            case "creditscore720":
                return product.CreditScore >= 720;
            case "productname7-1arm":
                return product.Name.ToLower() == "7-1 arm";
            default:
                Console.WriteLine($"Unknown condition: {condition}");
                return false;
        }
    }

    // Apply an action to a product
    private void ApplyAction(string action, Product product, double actionValue)
    {
        // Implement logic to apply actions
        switch (action.ToLower())
        {
            case "disqualify":
                product.Disqualified = true;
                break;
            case "reduceinterest":
                product.InterestRate -= actionValue;
                break;
            case "increaseinterest":
                product.InterestRate += actionValue;
                break;
            default:
                Console.WriteLine($"Unknown action: {action}");
                break;
        }
    }
}

class Program
{
    static void Main()
    {
        // Specify the path to the JSON file containing pricing rules
        string jsonFilePath = "/Users/ugochukwuanyachebelu/Projects/visio/visio/rules.json";

        // Get user input for state and credit score
        Console.Write("Enter the state: ");
        string state = Console.ReadLine();

        Console.Write("Enter the credit score: ");
        if (!int.TryParse(Console.ReadLine(), out int creditScore))
        {
            Console.WriteLine("Invalid credit score. Please enter a valid number.");
            return;
        }

        // Example usage with user input
        Product myProduct = new Product
        {
            Name = "7-1 ARM",
            InterestRate = 5.0,
            State = state,
            CreditScore = creditScore
        };

        // Create an instance of PricingRules with the path to the JSON file
        PricingRules pricingRules = new PricingRules(jsonFilePath);

        // Apply pricing rules to the product
        pricingRules.ApplyRules(myProduct);

        // Display the result
        Console.WriteLine($"Final interest rate for {myProduct.Name}: {myProduct.InterestRate}");
        Console.WriteLine($"Product disqualified: {myProduct.Disqualified}");
    }
}