-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3307
-- Generation Time: Mar 16, 2025 at 09:55 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bookteldb`
--

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `FirstName` longtext NOT NULL,
  `LastName` longtext NOT NULL,
  `ContactNumber` longtext NOT NULL,
  `Email` longtext NOT NULL,
  `Password` longtext NOT NULL,
  `Role` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `FirstName`, `LastName`, `ContactNumber`, `Email`, `Password`, `Role`) VALUES
(2, 'John', 'Doe', '09123456789', 'johndoe@example.com', 'password123', 'Guest'),
(3, 'cristian', 'torrejos', '09232368502', 'cristian@gmail.com', '$2a$11$D5bZefZFg/x2DQQDwbrHAudeoTWs08wO/n4j6/PJ.5NX91ztuFgPe', 'Guest'),
(4, 'Xing', 'Xiang', '09123456789', 'Xing@gmail.com', 'Xing@2003', 'Guest'),
(5, 'test1', 'er', '09123456789', 'test1@gmail.com', 'test1@gmail.com', 'Guest'),
(6, 'Admin', 'Admin', '09123456789', 'admin@gmail.com', '$2a$11$tP16VxV8DRKSZgLVHLKoU.sVQ80j3OQOp5inY/QTOjNY1j9sUSDWS', 'Admin');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20250313193929_InitialCreate', '8.0.13'),
('20250313195307_AddUserTable', '8.0.13'),
('20250313200131_FixUserModel', '8.0.13'),
('20250314165646_UpdateUsersTable', '8.0.13');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
