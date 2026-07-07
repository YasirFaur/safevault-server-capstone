# SafeVault Server (Capstone Project)

A secure backend web server built using C# and .NET Minimal APIs. This repository serves as the practical Capstone Project for the **Microsoft Back-End Developer Professional Certificate** program on Coursera.

---

## 🎯 Project Overview & Objective

The primary objective of **SafeVault Server** is to demonstrate practical mastery of core backend development principles, database integration, and critical web security practices. 

As part of a strategic milestone to bridge the gap between **Hardware (IoT / Embedded Systems)** and **Secure Backend Infrastructure**, this project applies industry-standard defensive programming. Modern IoT ecosystems require robust, highly secure servers to handle incoming telemetry data and prevent remote exploits. This project ensures data integrity from the edge devices to the database.

## 🎓 Educational Context

This repository represents the culmination of a rigorous **8-course specialization**:
* **Certification:** [Microsoft Back-End Developer Professional Certificate](https://www.coursera.org/professional-certificates/microsoft-back-end-developer)
* **Purpose:** Advancing engineering expertise in building secure, scalable backend applications, specifically tailored to support data logging and remote control architectures in advanced IoT and embedded electronics.

## 🛡️ Core Security Implementations

To fulfill the capstone requirements, the server focuses heavily on mitigating critical web vulnerabilities:

1. **Input Validation:** Prevents application crashes and logic errors by validating incoming data using robust C# structural validation (`string.IsNullOrWhiteSpace`).
2. **Cross-Site Scripting (XSS) Protection:** Utilizes `HtmlEncoder.Default.Encode` to sanitize user inputs, converting dangerous characters into safe HTML entities before processing.
3. **SQL Injection Mitigation:** Implements **Parameterized SQL Queries** (`SqlCommand` with parameters) to isolate user input from executable database logic, completely blocking injection vectors.

---
*Developed as part of a professional specialization in secure backend engineering and embedded solutions.*