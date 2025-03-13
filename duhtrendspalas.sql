-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 13, 2025 at 02:06 AM
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
-- Database: `duhtrendspalas`
--

-- --------------------------------------------------------

--
-- Table structure for table `brandpartner`
--

CREATE TABLE `brandpartner` (
  `BrandPartner_ID` int(11) NOT NULL,
  `BrandPartner_ContactNum` varchar(15) DEFAULT NULL,
  `BrandPartner_Email` varchar(100) DEFAULT NULL,
  `BrandPartner_Address` text DEFAULT NULL,
  `Firstname` varchar(100) DEFAULT NULL,
  `Lastname` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `brandpartner`
--

INSERT INTO `brandpartner` (`BrandPartner_ID`, `BrandPartner_ContactNum`, `BrandPartner_Email`, `BrandPartner_Address`, `Firstname`, `Lastname`) VALUES
(1, '09171234567', 'contact@techsolutions.com', '1234 Tech Street, Davao City', 'Techs', 'Inc.'),
(2, '09171234569', 'johhhn.doe@example.com', '2344 Tech Street, Davao City', 'John', 'Doe'),
(4, '09172345678', 'info@freshdelights.com', '7890 Market Road, Manila', 'Freshes', 'Delights'),
(13, '09457826124', 'k@gmail.com', 'Toril', 'Kristel Mae', 'Nacario'),
(14, '09251847965', 'a@gmail.com', 'Toril', 'Anne Marie', 'Amoroso'),
(17, '09876543876', 'm@gmail.com', 'Matina', 'Marchelle', 'Atienza'),
(18, '09875487658', 'abs@gmail.com', 'Baliok', 'Abegail', 'Shara'),
(19, '87650934876', 'acb@gmail.com', 'Baliok', 'Ansharimar', 'Balagosa'),
(20, '09876598764', 'h@gmail.com', 'Toril', 'Andriell', 'Balagosa'),
(22, '12345678901', 'qwerasdfzxcv', 'zxcvbnm', 'qwer', 'asdf'),
(23, '09872365253', 'Okay@gmail.com', 'Maa', 'Okay', 'Doks'),
(24, '09251459784', 'ja@gmail.com', 'Dona Luisa', 'Jervin', 'Andoy'),
(25, '', '', '', 'uyfu', ''),
(26, '2525', 'n', 'dvo', 'ju', 'j');

-- --------------------------------------------------------

--
-- Table structure for table `contract`
--

CREATE TABLE `contract` (
  `Contract_ID` int(11) NOT NULL,
  `BrandPartner_ID` int(11) DEFAULT NULL,
  `Owner_ID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `contract`
--

INSERT INTO `contract` (`Contract_ID`, `BrandPartner_ID`, `Owner_ID`) VALUES
(1, 1, 2),
(2, 2, 2),
(3, 4, 2),
(12, 13, 2),
(13, 14, 2),
(16, 17, 2),
(17, 18, 2),
(18, 19, 2),
(19, 20, 2),
(21, 22, 2),
(22, 23, 2),
(23, 24, 2),
(24, 25, 2),
(25, 26, 2);

-- --------------------------------------------------------

--
-- Table structure for table `employee`
--

CREATE TABLE `employee` (
  `Employee_ID` int(11) NOT NULL,
  `Firstname` varchar(50) NOT NULL,
  `Lastname` varchar(50) NOT NULL,
  `ContactNumber` varchar(15) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `user_level` enum('Owner','Admin','Cashier') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `employee`
--

INSERT INTO `employee` (`Employee_ID`, `Firstname`, `Lastname`, `ContactNumber`, `Address`, `email`, `is_active`, `user_level`) VALUES
(1, ' Rhy', ' Risaba', ' 0936115485', ' Baliok', ' sdsjw@gmail.com', 1, 'Admin'),
(2, 'Analita', 'Balagosa', 'ffy', 'Davao City', 'ail.com', 1, 'Admin'),
(3, 'Analita', 'Balagosa', '09251458697', 'Davao City', 'ail.com', 1, 'Admin'),
(10, 'Anne Klein', 'Amoroso', '09251485836', 'Matina', 'aka@gmail.com', 0, 'Cashier'),
(11, 'Analita', 'Balagosa', '09251458697', 'Davao City', 'an@gmail.com', 1, 'Admin'),
(12, 'Jonathan', 'May', '', 'Matina', 'giufiu', 1, 'Cashier');

-- --------------------------------------------------------

--
-- Table structure for table `login_history`
--

CREATE TABLE `login_history` (
  `id` int(11) NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `login_time` datetime NOT NULL,
  `logout_time` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `login_history`
--

INSERT INTO `login_history` (`id`, `employee_id`, `login_time`, `logout_time`) VALUES
(1, 1, '2025-02-18 01:32:20', '2025-02-18 01:33:22'),
(4, 1, '2025-02-18 13:56:59', '2025-02-18 13:57:26'),
(5, 1, '2025-02-18 14:24:48', '2025-02-18 14:25:02'),
(6, 1, '2025-02-18 14:26:36', '2025-02-18 14:27:15'),
(7, 1, '2025-02-18 14:37:23', '2025-02-18 14:37:39'),
(8, 1, '2025-02-18 15:22:53', '2025-02-18 15:25:58'),
(9, 1, '2025-02-19 22:35:29', '2025-02-19 22:37:26'),
(12, 1, '2025-02-19 23:03:44', '2025-02-19 23:04:24'),
(13, 1, '2025-02-19 23:07:40', '2025-02-19 23:08:55'),
(15, 1, '2025-02-23 14:22:03', '2025-02-23 14:23:11'),
(16, 1, '2025-02-23 15:11:30', '2025-02-23 15:12:52'),
(17, 1, '2025-02-23 15:23:41', '2025-02-23 15:24:57'),
(18, 1, '2025-02-23 15:33:15', '2025-02-23 15:37:01'),
(19, 1, '2025-02-23 15:40:03', NULL),
(20, 1, '2025-02-23 16:04:54', '2025-02-23 16:05:49'),
(21, 1, '2025-02-23 16:31:56', '2025-02-23 16:33:38'),
(22, 1, '2025-02-23 16:35:34', '2025-02-23 16:35:50'),
(24, 1, '2025-02-23 17:00:07', '2025-02-23 17:02:13'),
(25, 1, '2025-02-23 17:06:07', '2025-02-23 17:07:00'),
(26, 1, '2025-02-23 17:52:43', '2025-02-23 17:53:36'),
(28, 1, '2025-02-23 18:09:34', '2025-02-23 18:10:32'),
(29, 1, '2025-02-23 18:26:22', '2025-02-23 18:28:05'),
(35, 1, '2025-02-25 15:06:45', '2025-02-25 15:08:58'),
(36, 1, '2025-02-25 15:12:45', '2025-02-25 15:14:44'),
(37, 1, '2025-02-25 15:23:21', '2025-02-25 15:26:36'),
(39, 1, '2025-02-25 15:45:38', NULL),
(44, 1, '2025-02-25 17:22:02', '2025-02-25 17:23:14'),
(47, 1, '2025-02-25 18:33:01', '2025-02-25 18:33:16'),
(51, 1, '2025-02-25 20:54:08', '2025-02-25 20:54:36'),
(54, 1, '2025-02-25 21:54:48', '2025-02-25 22:07:11'),
(59, 1, '2025-02-25 23:19:07', '2025-02-25 23:20:18'),
(60, 1, '2025-02-25 23:29:41', '2025-02-25 23:34:12'),
(64, 1, '2025-02-25 23:42:05', '2025-02-25 23:43:04'),
(65, 3, '2025-02-26 14:11:08', '2025-02-26 14:12:01'),
(66, 3, '2025-02-26 15:00:07', '2025-02-26 15:00:26'),
(67, 3, '2025-02-26 15:49:41', '2025-02-26 15:55:06'),
(70, 3, '2025-02-26 16:27:33', NULL),
(71, 1, '2025-02-26 17:09:01', '2025-02-26 17:12:19'),
(73, 3, '2025-02-26 17:52:57', '2025-02-26 17:57:48'),
(74, 1, '2025-02-26 18:03:40', '2025-02-26 18:04:47'),
(76, 3, '2025-02-26 18:42:39', '2025-02-26 18:43:35'),
(77, 3, '2025-02-26 18:56:15', '2025-02-26 18:57:03'),
(78, 1, '2025-02-26 19:06:44', '2025-02-26 19:08:30'),
(79, 3, '2025-02-26 19:20:16', '2025-02-26 19:21:01'),
(82, 3, '2025-02-26 19:32:39', NULL),
(83, 3, '2025-02-26 19:50:26', '2025-02-26 19:50:39'),
(84, 3, '2025-02-26 20:04:18', '2025-02-26 20:04:24'),
(85, 1, '2025-02-26 20:04:37', '2025-02-26 20:04:40'),
(86, 3, '2025-02-26 20:06:17', '2025-02-26 20:07:14'),
(87, 1, '2025-02-26 20:09:16', '2025-02-26 20:09:23'),
(88, 1, '2025-02-26 20:16:40', '2025-02-26 20:16:50'),
(90, 3, '2025-02-26 20:21:20', NULL),
(91, 3, '2025-02-26 20:56:59', '2025-02-26 20:57:07'),
(92, 3, '2025-02-26 20:57:38', '2025-02-26 20:57:43'),
(93, 1, '2025-02-26 20:57:56', '2025-02-26 20:58:09'),
(94, 3, '2025-02-26 20:58:22', NULL),
(96, 3, '2025-02-26 23:11:17', '2025-02-26 23:11:25'),
(98, 1, '2025-02-26 23:40:55', '2025-02-26 23:45:18'),
(100, 3, '2025-03-02 17:50:18', '2025-03-02 17:54:03'),
(101, 3, '2025-03-02 19:11:28', '2025-03-02 19:26:51'),
(103, 3, '2025-03-02 19:47:51', '2025-03-02 19:52:56'),
(104, 3, '2025-03-02 20:02:16', '2025-03-02 20:09:47'),
(105, 1, '2025-03-02 20:32:29', '2025-03-02 20:33:10'),
(106, 1, '2025-03-02 20:33:27', '2025-03-02 20:39:10'),
(109, 1, '2025-03-02 23:02:47', '2025-03-02 23:11:04'),
(111, 1, '2025-03-03 00:34:04', '2025-03-03 00:41:20'),
(113, 3, '2025-03-03 01:08:38', NULL),
(114, 3, '2025-03-03 01:40:53', '2025-03-03 01:41:19'),
(117, 3, '2025-03-03 22:29:51', '2025-03-03 22:30:27'),
(121, 1, '2025-03-03 23:02:57', '2025-03-03 23:03:30'),
(122, 1, '2025-03-03 23:20:38', '2025-03-03 23:28:35'),
(123, 1, '2025-03-03 23:35:06', '2025-03-03 23:39:19'),
(124, 3, '2025-03-04 12:44:21', '2025-03-04 12:44:59'),
(125, 3, '2025-03-04 12:55:15', '2025-03-04 12:55:43'),
(126, 1, '2025-03-04 12:57:58', '2025-03-04 12:58:38'),
(127, 3, '2025-03-04 13:58:08', '2025-03-04 14:01:40'),
(128, 1, '2025-03-04 14:11:24', NULL),
(129, 3, '2025-03-04 14:13:18', '2025-03-04 14:21:26'),
(130, 1, '2025-03-05 14:50:22', '2025-03-05 14:53:15'),
(132, 3, '2025-03-05 15:31:08', '2025-03-05 15:35:23'),
(133, 1, '2025-03-05 16:03:41', '2025-03-05 16:05:49'),
(134, 3, '2025-03-05 16:23:21', NULL),
(135, 3, '2025-03-05 16:24:10', NULL),
(136, 1, '2025-03-05 16:47:56', '2025-03-05 16:49:18'),
(137, 3, '2025-03-05 16:55:46', '2025-03-05 16:56:15'),
(138, 3, '2025-03-05 16:59:37', '2025-03-05 16:59:46'),
(139, 3, '2025-03-05 17:18:32', '2025-03-05 17:18:59'),
(140, 1, '2025-03-05 17:22:10', '2025-03-05 17:22:40'),
(141, 1, '2025-03-05 17:25:16', '2025-03-05 17:25:43'),
(142, 3, '2025-03-05 17:35:24', '2025-03-05 17:35:53'),
(143, 3, '2025-03-05 17:56:53', NULL),
(144, 1, '2025-03-05 18:46:56', '2025-03-05 18:47:06'),
(145, 1, '2025-03-05 18:51:18', '2025-03-05 18:51:29'),
(146, 3, '2025-03-05 18:59:40', '2025-03-05 19:02:59'),
(147, 3, '2025-03-05 19:06:58', NULL),
(148, 3, '2025-03-05 19:27:27', '2025-03-05 19:29:09'),
(149, 3, '2025-03-05 19:34:24', '2025-03-05 19:37:13'),
(150, 1, '2025-03-05 21:06:44', '2025-03-05 21:14:44'),
(151, 3, '2025-03-06 21:49:22', '2025-03-06 22:01:01'),
(152, 1, '2025-03-06 22:02:04', '2025-03-06 22:23:36'),
(153, 1, '2025-03-06 22:24:45', '2025-03-06 22:31:43'),
(154, 1, '2025-03-06 22:33:42', '2025-03-06 22:37:59'),
(155, 3, '2025-03-06 22:48:46', '2025-03-06 23:32:17'),
(156, 1, '2025-03-06 23:32:45', '2025-03-06 23:36:42'),
(157, 3, '2025-03-06 23:40:37', '2025-03-07 00:01:05'),
(158, 1, '2025-03-07 00:24:31', NULL),
(159, 3, '2025-03-07 00:30:04', '2025-03-07 00:36:31'),
(160, 1, '2025-03-07 00:45:05', '2025-03-07 01:39:04'),
(161, 1, '2025-03-07 01:54:47', '2025-03-07 01:55:35'),
(162, 3, '2025-03-07 01:59:58', '2025-03-07 02:00:20'),
(163, 1, '2025-03-07 02:02:27', '2025-03-07 02:02:48'),
(164, 3, '2025-03-07 02:03:24', '2025-03-07 02:06:09'),
(165, 1, '2025-03-07 02:06:44', '2025-03-07 02:13:15'),
(166, 3, '2025-03-07 02:16:17', '2025-03-07 02:22:15'),
(167, 1, '2025-03-07 02:29:41', NULL),
(168, 3, '2025-03-07 02:37:20', '2025-03-07 02:39:31'),
(169, 1, '2025-03-07 02:43:31', '2025-03-07 02:45:47'),
(170, 1, '2025-03-07 02:50:08', NULL),
(171, 3, '2025-03-07 02:53:32', '2025-03-07 02:56:10'),
(172, 3, '2025-03-07 02:56:50', '2025-03-07 02:59:19'),
(173, 1, '2025-03-07 15:39:05', '2025-03-07 15:57:30'),
(174, 1, '2025-03-07 16:02:06', '2025-03-07 16:20:20'),
(175, 1, '2025-03-07 16:41:49', '2025-03-07 16:42:52'),
(176, 3, '2025-03-07 16:59:25', NULL),
(177, 1, '2025-03-07 17:24:41', '2025-03-07 17:26:26'),
(178, 3, '2025-03-07 18:41:08', '2025-03-07 18:41:37'),
(179, 3, '2025-03-07 18:43:06', '2025-03-07 18:58:43'),
(180, 1, '2025-03-07 19:22:55', '2025-03-07 19:27:23'),
(181, 3, '2025-03-07 19:33:10', '2025-03-07 19:34:28'),
(182, 3, '2025-03-08 16:05:31', '2025-03-08 16:08:42'),
(183, 3, '2025-03-08 16:49:41', '2025-03-08 16:55:29'),
(184, 1, '2025-03-08 17:13:07', '2025-03-08 17:15:56'),
(185, 3, '2025-03-08 17:23:17', '2025-03-08 17:24:52'),
(186, 3, '2025-03-08 18:12:17', '2025-03-08 18:14:00'),
(187, 1, '2025-03-08 18:17:40', '2025-03-08 18:18:03'),
(188, 1, '2025-03-08 18:21:48', '2025-03-08 18:22:28'),
(189, 3, '2025-03-08 18:27:08', '2025-03-08 18:28:47'),
(190, 3, '2025-03-08 18:33:31', '2025-03-08 19:06:45'),
(191, 3, '2025-03-08 19:40:58', '2025-03-08 19:49:14'),
(192, 1, '2025-03-08 20:01:14', '2025-03-08 20:04:16'),
(193, 1, '2025-03-08 20:07:19', '2025-03-08 20:16:58'),
(194, 1, '2025-03-08 20:45:27', '2025-03-08 20:47:40'),
(195, 1, '2025-03-08 21:02:42', '2025-03-08 21:04:33'),
(196, 3, '2025-03-08 21:17:58', '2025-03-08 21:20:34'),
(197, 3, '2025-03-08 22:31:57', '2025-03-08 22:33:22'),
(198, 3, '2025-03-08 22:48:59', '2025-03-08 22:49:22'),
(199, 3, '2025-03-08 22:51:40', '2025-03-08 22:52:20'),
(200, 1, '2025-03-08 23:05:29', '2025-03-08 23:06:35'),
(201, 3, '2025-03-08 23:27:15', '2025-03-08 23:27:56'),
(202, 3, '2025-03-08 23:34:14', '2025-03-08 23:35:50'),
(203, 1, '2025-03-08 23:53:07', '2025-03-09 00:00:11'),
(204, 3, '2025-03-09 11:04:53', '2025-03-09 11:16:09'),
(205, 3, '2025-03-09 11:43:27', '2025-03-09 11:48:36'),
(206, 3, '2025-03-09 11:49:22', '2025-03-09 11:50:52'),
(207, 1, '2025-03-09 12:18:38', '2025-03-09 12:19:26'),
(208, 3, '2025-03-09 12:31:44', '2025-03-09 12:32:15'),
(209, 3, '2025-03-09 12:41:47', '2025-03-09 12:42:25'),
(210, 1, '2025-03-09 12:46:52', '2025-03-09 12:47:34'),
(211, 3, '2025-03-09 12:55:02', '2025-03-09 12:55:19'),
(212, 1, '2025-03-09 13:21:58', '2025-03-09 13:22:29'),
(213, 3, '2025-03-09 13:27:52', '2025-03-09 13:28:41'),
(214, 1, '2025-03-09 13:35:18', NULL),
(215, 3, '2025-03-09 13:43:28', '2025-03-09 13:45:21'),
(216, 3, '2025-03-09 13:48:20', '2025-03-09 13:49:12'),
(217, 3, '2025-03-09 13:53:26', '2025-03-09 13:56:58'),
(218, 1, '2025-03-09 13:58:00', '2025-03-09 14:02:33'),
(219, 3, '2025-03-09 14:09:44', '2025-03-09 14:10:44'),
(220, 1, '2025-03-09 15:05:03', '2025-03-09 15:07:08'),
(221, 3, '2025-03-09 15:36:46', '2025-03-09 15:37:36'),
(222, 3, '2025-03-09 15:40:31', '2025-03-09 15:41:48'),
(223, 3, '2025-03-09 15:45:37', '2025-03-09 15:46:22'),
(224, 1, '2025-03-09 16:51:05', '2025-03-09 16:51:34'),
(225, 3, '2025-03-09 16:56:18', '2025-03-09 17:06:50'),
(226, 3, '2025-03-09 20:25:10', '2025-03-09 20:40:34'),
(227, 1, '2025-03-09 21:33:57', '2025-03-09 21:35:02'),
(228, 3, '2025-03-09 21:38:17', '2025-03-09 21:41:36'),
(229, 3, '2025-03-09 21:53:01', '2025-03-09 21:54:43'),
(230, 3, '2025-03-09 21:57:27', '2025-03-09 21:58:46'),
(231, 1, '2025-03-09 21:59:08', '2025-03-09 22:01:26'),
(232, 3, '2025-03-09 22:20:07', '2025-03-09 22:35:30'),
(233, 3, '2025-03-09 22:38:30', '2025-03-09 22:46:47'),
(234, 3, '2025-03-09 22:54:16', '2025-03-09 23:00:28'),
(235, 1, '2025-03-09 23:05:52', '2025-03-09 23:07:27'),
(236, 1, '2025-03-09 23:17:55', '2025-03-09 23:20:36'),
(237, 1, '2025-03-09 23:35:37', '2025-03-09 23:38:49'),
(238, 3, '2025-03-11 10:38:52', '2025-03-11 10:44:47'),
(239, 3, '2025-03-11 11:34:53', '2025-03-11 11:35:05'),
(240, 1, '2025-03-11 11:35:27', '2025-03-11 11:43:17'),
(241, 1, '2025-03-11 11:56:21', '2025-03-11 12:08:30'),
(242, 1, '2025-03-11 12:57:40', '2025-03-11 13:02:09'),
(243, 1, '2025-03-11 13:17:50', '2025-03-11 13:30:05'),
(244, 1, '2025-03-11 13:30:58', '2025-03-11 13:32:27'),
(245, 1, '2025-03-11 13:43:48', NULL),
(246, 1, '2025-03-11 14:16:57', NULL),
(247, 1, '2025-03-11 14:17:23', '2025-03-11 14:18:51'),
(248, 1, '2025-03-11 14:54:07', '2025-03-11 14:57:36'),
(249, 1, '2025-03-11 15:12:19', '2025-03-11 15:17:28'),
(250, 1, '2025-03-11 15:19:12', '2025-03-11 15:24:59'),
(251, 1, '2025-03-11 15:29:58', '2025-03-11 15:45:22'),
(252, 1, '2025-03-11 15:48:39', '2025-03-11 15:57:00'),
(253, 3, '2025-03-11 15:57:34', '2025-03-11 16:12:33'),
(254, 1, '2025-03-11 16:42:48', '2025-03-11 16:47:56'),
(255, 1, '2025-03-11 17:12:59', '2025-03-11 17:14:00'),
(256, 1, '2025-03-11 18:08:27', '2025-03-11 18:15:15'),
(257, 1, '2025-03-11 18:16:10', '2025-03-11 18:30:49'),
(258, 1, '2025-03-11 19:22:04', '2025-03-11 19:23:43'),
(259, 1, '2025-03-11 19:38:11', '2025-03-11 19:38:37'),
(260, 3, '2025-03-11 19:58:03', '2025-03-11 20:03:00'),
(261, 1, '2025-03-11 20:08:54', '2025-03-11 20:20:10'),
(262, 1, '2025-03-11 20:23:58', '2025-03-11 20:24:23'),
(263, 1, '2025-03-11 20:46:51', '2025-03-11 20:48:28'),
(264, 1, '2025-03-11 22:04:33', '2025-03-11 22:04:57'),
(265, 1, '2025-03-11 22:59:21', '2025-03-11 23:04:47'),
(266, 1, '2025-03-11 23:29:04', '2025-03-11 23:32:05'),
(267, 1, '2025-03-11 23:44:45', '2025-03-11 23:46:48'),
(268, 1, '2025-03-11 23:51:33', '2025-03-11 23:52:32'),
(269, 1, '2025-03-12 00:05:13', '2025-03-12 00:06:03'),
(270, 1, '2025-03-12 09:22:29', NULL),
(271, 1, '2025-03-12 09:41:38', '2025-03-12 09:52:29'),
(272, 3, '2025-03-12 10:23:21', '2025-03-12 10:23:31'),
(273, 3, '2025-03-12 10:24:21', NULL),
(274, 3, '2025-03-12 10:25:34', '2025-03-12 10:33:50'),
(275, 1, '2025-03-12 10:34:18', '2025-03-12 10:34:29'),
(276, 1, '2025-03-12 10:34:50', NULL),
(277, 1, '2025-03-12 10:58:40', '2025-03-12 11:01:11'),
(278, 1, '2025-03-12 11:20:58', '2025-03-12 11:34:36'),
(279, 1, '2025-03-12 11:46:28', '2025-03-12 12:15:59'),
(280, 1, '2025-03-12 12:35:08', '2025-03-12 12:42:42'),
(281, 1, '2025-03-12 12:57:05', '2025-03-12 12:59:32'),
(282, 1, '2025-03-12 13:02:03', '2025-03-12 13:02:38'),
(283, 1, '2025-03-12 13:44:51', '2025-03-12 13:46:03'),
(284, 1, '2025-03-12 13:57:09', '2025-03-12 13:57:55'),
(285, 1, '2025-03-12 14:18:36', '2025-03-12 14:33:15'),
(286, 1, '2025-03-12 19:13:02', '2025-03-12 19:13:49'),
(287, 1, '2025-03-12 23:46:53', '2025-03-12 23:47:16');

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `order_id` int(11) NOT NULL,
  `order_date` date DEFAULT curdate(),
  `employee_id` int(11) NOT NULL,
  `total` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`order_id`, `order_date`, `employee_id`, `total`) VALUES
(11, '2025-02-23', 1, 6600.00),
(12, '2025-02-23', 1, 3600.00),
(13, '2025-02-23', 1, 689.92),
(14, '2025-02-23', 1, 7519.96),
(15, '2025-02-23', 2, 10500.00),
(16, '2025-02-23', 2, 9429.94),
(17, '2025-02-25', 2, 969.91),
(21, '2025-02-25', 2, 9089.99),
(22, '2025-02-25', 2, 3500.00),
(23, '2025-02-25', 2, 3500.00),
(24, '2025-02-25', 2, 59.99),
(25, '2025-02-25', 2, 129.99),
(26, '2025-02-25', 2, 1200.00),
(27, '2025-02-25', 2, 59.99),
(28, '2025-02-25', 2, 59.99),
(29, '2025-02-26', 3, 1200.00),
(30, '2025-02-26', 3, 1200.00),
(31, '2025-02-26', 1, 1200.00),
(32, '2025-02-26', 2, 1200.00),
(33, '2025-02-26', 3, 59.99),
(34, '2025-02-26', 1, 3500.00),
(35, '2025-02-26', 3, 3500.00),
(36, '2025-02-26', 2, 59.99),
(37, '2025-02-26', 2, 59.99),
(38, '2025-02-26', 3, 59.99),
(39, '2025-02-26', 3, 5000.00),
(40, '2025-02-26', 1, 6000.00),
(41, '2025-02-26', 2, 3500.00),
(42, '2025-02-26', 3, 1800.00),
(43, '2025-02-26', 1, 59.99),
(44, '2025-02-26', 2, 5000.00),
(45, '2025-02-26', 1, 7000.00),
(47, '2025-03-02', 2, 6000.00),
(48, '2025-03-03', 2, 1500.00),
(49, '2025-03-07', 1, 3300.00),
(50, '2025-03-07', 3, 11000.00),
(53, '2025-03-08', 3, 7000.00),
(54, '2025-03-08', 1, 7000.00),
(57, '2025-03-08', 3, 1500.00),
(58, '2025-03-08', 3, 1500.00),
(59, '2025-03-08', 3, 1500.00),
(60, '2025-03-08', 3, 1500.00),
(61, '2025-03-08', 3, 59.99),
(62, '2025-03-08', 3, 5000.00),
(63, '2025-03-08', 3, 5059.99),
(64, '2025-03-08', 3, 179.98),
(67, '2025-03-08', 1, 3500.00),
(68, '2025-03-08', 1, 59.99),
(73, '2025-03-08', 1, 3500.00),
(74, '2025-03-08', 1, 89.99),
(75, '2025-03-08', 1, 179.97),
(76, '2025-03-08', 3, 3500.00),
(79, '2025-03-08', 3, 4500.00),
(80, '2025-03-08', 3, 3500.00),
(83, '2025-03-08', 1, 1500.00),
(84, '2025-03-08', 1, 1500.00),
(85, '2025-03-08', 3, 3500.00),
(86, '2025-03-08', 3, 3000.00),
(87, '2025-03-08', 3, 3000.00),
(88, '2025-03-08', 1, 3500.00),
(89, '2025-03-08', 1, 179.97),
(90, '2025-03-09', 1, 5000.00),
(91, '2025-03-09', 3, 14500.00),
(92, '2025-03-09', 3, 10000.00),
(93, '2025-03-09', 3, 3500.00),
(94, '2025-03-09', 1, 3500.00),
(95, '2025-03-09', 1, 4500.00),
(96, '2025-03-09', 3, 5119.98),
(97, '2025-03-11', 1, 90.00),
(98, '2025-03-11', 1, 250.00),
(99, '2025-03-12', 1, 100150.00),
(100, '2025-03-12', 1, 550000.00),
(101, '2025-03-12', 3, 1429.89),
(102, '2025-03-12', 3, 1500.00),
(103, '2025-03-12', 3, 1000.00),
(104, '2025-03-12', 1, 49315.00),
(105, '2025-03-12', 1, 14400.00);

-- --------------------------------------------------------

--
-- Table structure for table `order_details`
--

CREATE TABLE `order_details` (
  `order_detail_id` int(11) NOT NULL,
  `order_id` int(11) NOT NULL,
  `product_barcode` varchar(50) NOT NULL,
  `quantity` int(11) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `subtotal` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `order_details`
--

INSERT INTO `order_details` (`order_detail_id`, `order_id`, `product_barcode`, `quantity`, `price`, `subtotal`) VALUES
(1, 11, '223456789013', 3, 1800.00, 0.00),
(2, 11, '223456789012', 1, 1200.00, 0.00),
(3, 12, '223456789013', 2, 1800.00, 0.00),
(4, 13, 'FOOD2002', 5, 59.99, 0.00),
(5, 13, 'FOOD2003', 3, 129.99, 0.00),
(6, 14, 'FOOD2003', 4, 129.99, 0.00),
(7, 14, '123456789014', 2, 3500.00, 0.00),
(8, 15, '123456789014', 3, 3500.00, 0.00),
(9, 16, 'FOOD2003', 1, 129.99, 0.00),
(10, 16, '223456789016', 2, 4500.00, 0.00),
(11, 16, 'FOOD2002', 5, 59.99, 0.00),
(12, 17, 'FOOD2001', 5, 89.99, 0.00),
(13, 17, 'FOOD2003', 4, 129.99, 0.00),
(14, 21, 'FOOD2001', 1, 89.99, 89.99),
(15, 21, '223456789016', 2, 4500.00, 9000.00),
(16, 22, '123456789014', 1, 3500.00, 3500.00),
(17, 23, '123456789014', 1, 3500.00, 3500.00),
(18, 24, 'FOOD2002', 1, 59.99, 59.99),
(19, 25, 'FOOD2003', 1, 129.99, 129.99),
(20, 26, '223456789012', 1, 1200.00, 1200.00),
(21, 27, 'FOOD2002', 1, 59.99, 59.99),
(22, 28, 'FOOD2002', 1, 59.99, 59.99),
(23, 29, '223456789012', 1, 1200.00, 1200.00),
(24, 30, '223456789012', 1, 1200.00, 1200.00),
(25, 31, '223456789012', 1, 1200.00, 1200.00),
(26, 32, '223456789012', 1, 1200.00, 1200.00),
(27, 33, 'FOOD2002', 1, 59.99, 59.99),
(28, 34, '123456789014', 1, 3500.00, 3500.00),
(29, 35, '123456789014', 1, 3500.00, 3500.00),
(30, 36, 'FOOD2002', 1, 59.99, 59.99),
(31, 37, 'FOOD2002', 1, 59.99, 59.99),
(32, 38, 'FOOD2002', 1, 59.99, 59.99),
(33, 39, '123456789015', 1, 5000.00, 5000.00),
(34, 40, '123456789016', 1, 6000.00, 6000.00),
(35, 41, '123456789014', 1, 3500.00, 3500.00),
(36, 42, '223456789013', 1, 1800.00, 1800.00),
(37, 43, 'FOOD2002', 1, 59.99, 59.99),
(38, 44, '123456789015', 1, 5000.00, 5000.00),
(39, 45, '123456789014', 2, 3500.00, 7000.00),
(40, 47, '123456789016', 1, 6000.00, 6000.00),
(41, 48, '123456789082', 1, 1500.00, 1500.00),
(42, 49, '223456789013', 1, 1800.00, 1800.00),
(43, 49, '123456789082', 1, 1500.00, 1500.00),
(44, 50, '123456789015', 1, 5000.00, 5000.00),
(45, 50, '123456789016', 1, 6000.00, 6000.00),
(46, 53, '123456789014', 2, 3500.00, 7000.00),
(47, 54, '123456789014', 2, 3500.00, 7000.00),
(48, 57, '123456789082', 1, 1500.00, 1500.00),
(49, 58, '123456789082', 1, 1500.00, 1500.00),
(50, 59, '123456789082', 1, 1500.00, 1500.00),
(51, 60, '123456789082', 1, 1500.00, 1500.00),
(52, 61, 'FOOD2002', 1, 59.99, 59.99),
(53, 62, '123456789015', 1, 5000.00, 5000.00),
(54, 63, '123456789015', 1, 5000.00, 5000.00),
(55, 63, 'FOOD2002', 1, 59.99, 59.99),
(56, 64, 'FOOD2001', 2, 89.99, 179.98),
(57, 67, '123456789014', 1, 3500.00, 3500.00),
(58, 68, 'FOOD2002', 1, 59.99, 59.99),
(59, 73, '123456789014', 1, 3500.00, 3500.00),
(60, 74, 'FOOD2001', 1, 89.99, 89.99),
(61, 75, 'FOOD2002', 3, 59.99, 179.97),
(62, 76, '123456789014', 1, 3500.00, 3500.00),
(63, 79, '223456789016', 1, 4500.00, 4500.00),
(64, 80, '123456789014', 1, 3500.00, 3500.00),
(65, 83, '123456789012', 1, 1500.00, 1500.00),
(66, 84, '123456789012', 1, 1500.00, 1500.00),
(67, 85, '123456789014', 1, 3500.00, 3500.00),
(68, 86, '123456789012', 2, 1500.00, 3000.00),
(69, 87, '123456789012', 2, 1500.00, 3000.00),
(70, 88, '123456789014', 1, 3500.00, 3500.00),
(71, 89, 'FOOD2002', 3, 59.99, 179.97),
(72, 90, '123456789015', 1, 5000.00, 5000.00),
(73, 91, '123456789015', 2, 5000.00, 10000.00),
(74, 91, '123456789012', 3, 1500.00, 4500.00),
(75, 92, '123456789015', 2, 5000.00, 10000.00),
(76, 93, '123456789014', 1, 3500.00, 3500.00),
(77, 94, '123456789014', 1, 3500.00, 3500.00),
(78, 95, '223456789016', 1, 4500.00, 4500.00),
(79, 96, '123456789013', 2, 2500.00, 5000.00),
(80, 96, 'FOOD2002', 2, 59.99, 119.98),
(81, 97, '678456', 2, 45.00, 90.00),
(82, 98, '123456', 5, 50.00, 250.00),
(83, 99, '223456789015', 50, 2000.00, 100000.00),
(84, 99, '123456', 3, 50.00, 150.00),
(85, 100, '223456789014', 250, 2200.00, 550000.00),
(86, 101, 'FOOD2003', 11, 129.99, 1429.89),
(87, 102, 'FOOD2002', 25, 60.00, 1500.00),
(88, 103, '34534534', 20, 50.00, 1000.00),
(89, 104, '223456789016', 7, 4500.00, 31500.00),
(90, 104, '678456', 7, 45.00, 315.00),
(91, 104, '123456789014', 5, 3500.00, 17500.00),
(92, 105, '223456789013', 8, 1800.00, 14400.00);

--
-- Triggers `order_details`
--
DELIMITER $$
CREATE TRIGGER `after_order_insert` AFTER INSERT ON `order_details` FOR EACH ROW BEGIN
UPDATE product
SET quantity = quantity - NEW.quantity
WHERE product_barcode = NEW.product_barcode;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `owner`
--

CREATE TABLE `owner` (
  `Owner_ID` int(11) NOT NULL,
  `Employee_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `owner`
--

INSERT INTO `owner` (`Owner_ID`, `Employee_ID`) VALUES
(2, 3);

-- --------------------------------------------------------

--
-- Table structure for table `product`
--

CREATE TABLE `product` (
  `product_barcode` varchar(50) NOT NULL,
  `product_name` varchar(100) DEFAULT NULL,
  `brandpartner_id` int(11) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  `price` decimal(10,2) DEFAULT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `delivery_date` datetime DEFAULT current_timestamp(),
  `expiration_date` datetime DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `product`
--

INSERT INTO `product` (`product_barcode`, `product_name`, `brandpartner_id`, `quantity`, `price`, `employee_id`, `delivery_date`, `expiration_date`, `is_active`) VALUES
('123', 'Matt', 18, 12, 25.00, 1, '2025-03-12 11:51:28', NULL, 0),
('12312312', 'Bread', 20, 10, 85.00, 1, '2025-03-11 14:55:24', '2026-05-22 14:55:24', 0),
('123456', 'Liptint', 13, 10, 50.00, 10, '2025-03-11 18:16:35', '2025-05-01 18:16:35', 1),
('123456789012', 'Tech Gadgets', 1, 85, 1500.00, 1, '2025-02-18 15:13:22', NULL, 1),
('123456789013', 'Tech Gadget B', 1, 48, 2500.00, 1, '2025-02-18 15:14:37', NULL, 1),
('123456789014', 'Tech Gadget C', 14, 147, 3500.00, 1, '2025-02-18 15:14:37', NULL, 1),
('123456789015', 'Tech Phone X', 1, 60, 5000.00, 1, '2025-02-18 15:14:37', NULL, 1),
('123456789016', 'Tech Phone Y', 1, 19, 6000.00, 1, '2025-02-18 15:14:37', NULL, 0),
('123456789082', 'Tech Gadget A', 1, 89, 1500.00, 1, '2025-02-18 15:14:37', NULL, 1),
('12345678998', 'Perfume', 18, 15, 200.00, NULL, '2025-03-11 13:43:58', '2025-08-23 00:00:00', 0),
('223456789012', 'Gadget Pro A', 2, 130, 1200.00, 2, '2025-02-18 15:15:03', NULL, 1),
('223456789013', 'Gadget Pro B', 2, 37, 1800.00, 2, '2025-02-18 15:15:03', NULL, 1),
('223456789014', 'Gadget Pro C', 2, 50, 2200.00, 2, '2025-02-18 15:15:03', NULL, 1),
('223456789015', 'Gadget Pro X', 2, 40, 2000.00, 2, '2025-02-18 15:15:03', NULL, 1),
('223456789016', 'Gadget Pro Y', 2, 30, 4500.00, 2, '2025-02-18 15:15:03', NULL, 1),
('34534534', 'Cookies', 14, 50, 50.00, 1, '2025-03-11 15:20:45', '2025-03-27 20:11:54', 1),
('454523', 'Pillow', 1, 10, 135.00, 1, '2025-03-11 20:09:57', NULL, 1),
('456789', 'Mouse', 22, 25, 200.00, 3, '2025-03-12 12:06:28', NULL, 1),
('678456', 'Mango', 17, 3, 45.00, 3, '2025-03-11 15:49:01', NULL, 1),
('FOOD2001', 'Organic Milk', 4, 49, 89.99, 3, '2025-02-23 17:59:45', '2027-09-30 19:59:06', 1),
('FOOD2002', 'Whole Wheat Bread', 13, 25, 60.00, NULL, '2025-02-23 17:59:45', '2025-04-30 00:00:00', 1),
('FOOD2003', 'Dark Chocolate Bar', 4, 10, 129.99, NULL, '2025-02-23 17:59:45', '2026-01-10 00:00:00', 1);

-- --------------------------------------------------------

--
-- Table structure for table `product_pullout`
--

CREATE TABLE `product_pullout` (
  `pullout_id` int(11) NOT NULL,
  `product_barcode` varchar(50) NOT NULL,
  `quantity_pulled` int(11) NOT NULL,
  `reason` varchar(50) NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `pullout_date` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `product_pullout`
--

INSERT INTO `product_pullout` (`pullout_id`, `product_barcode`, `quantity_pulled`, `reason`, `employee_id`, `pullout_date`) VALUES
(3, '123456', 1, 'Lost', 1, '2025-03-11 23:51:48'),
(4, '678456', 13, 'Defective', 1, '2025-03-12 09:34:50'),
(5, 'FOOD2001', 1, 'Defective', 1, '2025-03-12 14:23:59');

-- --------------------------------------------------------

--
-- Table structure for table `storagetype`
--

CREATE TABLE `storagetype` (
  `Storage_ID` int(11) NOT NULL,
  `Storage_price` decimal(10,2) NOT NULL CHECK (`Storage_price` in (1000,1200,1500,2200,2500,3000,3500,5500)),
  `ContractID` int(11) DEFAULT NULL,
  `Contract_startdate` date DEFAULT NULL,
  `Contract_enddate` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `storagetype`
--

INSERT INTO `storagetype` (`Storage_ID`, `Storage_price`, `ContractID`, `Contract_startdate`, `Contract_enddate`) VALUES
(2, 2500.00, 1, '2025-01-01', '2025-12-31'),
(6, 3000.00, 3, '2025-01-01', '2025-12-31'),
(7, 5500.00, 2, '2025-01-01', '2025-12-31'),
(8, 2500.00, 1, '2025-01-01', '2025-12-31'),
(21, 5500.00, 12, '2025-03-03', '2025-04-28'),
(26, 1500.00, 2, '2025-01-01', '2025-12-31'),
(27, 3000.00, 17, '2025-03-06', '2025-09-26'),
(28, 2500.00, 17, '2025-02-06', '2025-05-17'),
(31, 1200.00, 16, '2025-03-06', '2025-03-29'),
(32, 1200.00, 16, '2025-03-06', '2025-04-18'),
(33, 1500.00, 13, '2025-03-03', '2025-06-19'),
(38, 5500.00, 19, '2025-03-06', '2025-05-31'),
(40, 1000.00, 13, '2025-03-03', '2025-06-19'),
(41, 1500.00, 22, '2025-03-07', '2025-04-26'),
(42, 1000.00, 22, '2025-03-07', '2025-04-26'),
(45, 2200.00, 18, '2025-03-21', '2025-06-28'),
(46, 3500.00, 23, '2025-03-31', '2025-12-31'),
(47, 1000.00, 23, '2025-03-31', '2025-12-31'),
(50, 1500.00, 21, '2025-03-07', '2025-05-31'),
(51, 1200.00, 24, '2025-03-12', '2025-03-12'),
(53, 2500.00, 25, '2025-03-12', '2025-05-23');

-- --------------------------------------------------------

--
-- Table structure for table `supply`
--

CREATE TABLE `supply` (
  `supply_id` int(11) NOT NULL,
  `BrandPartner_ID` int(11) DEFAULT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `supply_date` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `supply`
--

INSERT INTO `supply` (`supply_id`, `BrandPartner_ID`, `employee_id`, `supply_date`) VALUES
(5, 14, 2, '2025-03-11 13:18:34'),
(9, 18, 10, '2025-03-11 13:43:58'),
(10, 20, 1, '2025-03-11 14:55:24'),
(11, 14, 1, '2025-03-11 15:20:45'),
(12, 17, 3, '2025-03-11 15:49:01'),
(13, 13, 10, '2025-03-11 18:16:35'),
(14, 1, 1, '2025-03-11 20:09:57'),
(15, 1, 1, '2025-03-12 10:27:45'),
(16, 22, 3, '2025-03-12 10:35:03'),
(17, 18, 1, '2025-03-12 11:51:28'),
(18, 22, 3, '2025-03-12 12:06:28');

-- --------------------------------------------------------

--
-- Table structure for table `supply_details`
--

CREATE TABLE `supply_details` (
  `supply_id` int(11) NOT NULL,
  `product_barcode` varchar(50) NOT NULL,
  `product_name` varchar(100) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  `price` decimal(10,2) DEFAULT NULL,
  `expiration_date` date DEFAULT NULL,
  `supply_receivedby` varchar(255) NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `supply_details`
--

INSERT INTO `supply_details` (`supply_id`, `product_barcode`, `product_name`, `quantity`, `price`, `expiration_date`, `supply_receivedby`, `is_active`) VALUES
(5, '123456789012', 'Milk', 25, 50.00, '2025-05-30', 'Janedy Smith', 0),
(9, '12345678998', 'Perfume', 15, 200.00, '2025-08-23', 'Anne Klein Amoroso', 0),
(10, '12312312', 'Bread', 10, 85.00, '2026-05-22', '1', 0),
(11, '34534534', 'Cookies', 25, 50.00, '2025-08-23', 'John Doe', 1),
(12, '678456', 'Mango', 15, 45.00, '2025-06-20', 'Ansharlene Crystal Balagosa', 1),
(13, '123456', 'Liptint', 10, 50.00, '2025-05-01', 'Anne Klein Amoroso', 1),
(14, '454523', 'Pillow', 15, 135.00, '2025-04-18', 'John Doe', 1),
(17, '123', 'Matt', 12, 25.00, NULL, 'Rhy  Risaba', 0),
(18, '456789', 'Mouse', 25, 200.00, NULL, 'Analita Balagosa', 1);

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `employee_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`username`, `password`, `employee_id`) VALUES
('sample', 'sample', 1),
('ansharlene', 'ansharlene', 3);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `brandpartner`
--
ALTER TABLE `brandpartner`
  ADD PRIMARY KEY (`BrandPartner_ID`);

--
-- Indexes for table `contract`
--
ALTER TABLE `contract`
  ADD PRIMARY KEY (`Contract_ID`),
  ADD KEY `BrandPartner_ID` (`BrandPartner_ID`),
  ADD KEY `fk_owner` (`Owner_ID`);

--
-- Indexes for table `employee`
--
ALTER TABLE `employee`
  ADD PRIMARY KEY (`Employee_ID`);

--
-- Indexes for table `login_history`
--
ALTER TABLE `login_history`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_login_history` (`employee_id`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`order_id`),
  ADD KEY `employee_id` (`employee_id`);

--
-- Indexes for table `order_details`
--
ALTER TABLE `order_details`
  ADD PRIMARY KEY (`order_detail_id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `product_barcode` (`product_barcode`);

--
-- Indexes for table `owner`
--
ALTER TABLE `owner`
  ADD PRIMARY KEY (`Owner_ID`),
  ADD UNIQUE KEY `Employee_ID` (`Employee_ID`);

--
-- Indexes for table `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`product_barcode`),
  ADD KEY `employee_id` (`employee_id`),
  ADD KEY `product_ibfk_1` (`brandpartner_id`);

--
-- Indexes for table `product_pullout`
--
ALTER TABLE `product_pullout`
  ADD PRIMARY KEY (`pullout_id`),
  ADD KEY `product_barcode` (`product_barcode`),
  ADD KEY `employee_id` (`employee_id`);

--
-- Indexes for table `storagetype`
--
ALTER TABLE `storagetype`
  ADD PRIMARY KEY (`Storage_ID`),
  ADD KEY `ContractID` (`ContractID`);

--
-- Indexes for table `supply`
--
ALTER TABLE `supply`
  ADD PRIMARY KEY (`supply_id`),
  ADD KEY `employee_id` (`employee_id`),
  ADD KEY `fk_BrandPartner` (`BrandPartner_ID`);

--
-- Indexes for table `supply_details`
--
ALTER TABLE `supply_details`
  ADD PRIMARY KEY (`supply_id`,`product_barcode`),
  ADD KEY `fk_product` (`product_barcode`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`employee_id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `brandpartner`
--
ALTER TABLE `brandpartner`
  MODIFY `BrandPartner_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT for table `contract`
--
ALTER TABLE `contract`
  MODIFY `Contract_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT for table `employee`
--
ALTER TABLE `employee`
  MODIFY `Employee_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `login_history`
--
ALTER TABLE `login_history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=288;

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `order_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=106;

--
-- AUTO_INCREMENT for table `order_details`
--
ALTER TABLE `order_details`
  MODIFY `order_detail_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=93;

--
-- AUTO_INCREMENT for table `owner`
--
ALTER TABLE `owner`
  MODIFY `Owner_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `product_pullout`
--
ALTER TABLE `product_pullout`
  MODIFY `pullout_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `storagetype`
--
ALTER TABLE `storagetype`
  MODIFY `Storage_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT for table `supply`
--
ALTER TABLE `supply`
  MODIFY `supply_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `contract`
--
ALTER TABLE `contract`
  ADD CONSTRAINT `contract_ibfk_1` FOREIGN KEY (`BrandPartner_ID`) REFERENCES `brandpartner` (`BrandPartner_ID`),
  ADD CONSTRAINT `fk_contract_owner` FOREIGN KEY (`Owner_ID`) REFERENCES `owner` (`Owner_ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `login_history`
--
ALTER TABLE `login_history`
  ADD CONSTRAINT `fk_login_history` FOREIGN KEY (`employee_id`) REFERENCES `user` (`employee_id`) ON DELETE CASCADE;

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employee` (`Employee_ID`);

--
-- Constraints for table `order_details`
--
ALTER TABLE `order_details`
  ADD CONSTRAINT `order_details_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`order_id`),
  ADD CONSTRAINT `order_details_ibfk_2` FOREIGN KEY (`product_barcode`) REFERENCES `product` (`product_barcode`);

--
-- Constraints for table `owner`
--
ALTER TABLE `owner`
  ADD CONSTRAINT `owner_ibfk_1` FOREIGN KEY (`Employee_ID`) REFERENCES `employee` (`Employee_ID`) ON DELETE CASCADE;

--
-- Constraints for table `product`
--
ALTER TABLE `product`
  ADD CONSTRAINT `product_ibfk_1` FOREIGN KEY (`brandpartner_id`) REFERENCES `brandpartner` (`BrandPartner_ID`) ON DELETE CASCADE,
  ADD CONSTRAINT `product_ibfk_2` FOREIGN KEY (`employee_id`) REFERENCES `employee` (`Employee_ID`);

--
-- Constraints for table `product_pullout`
--
ALTER TABLE `product_pullout`
  ADD CONSTRAINT `product_pullout_ibfk_1` FOREIGN KEY (`product_barcode`) REFERENCES `product` (`product_barcode`) ON DELETE CASCADE,
  ADD CONSTRAINT `product_pullout_ibfk_2` FOREIGN KEY (`employee_id`) REFERENCES `employee` (`Employee_ID`) ON DELETE SET NULL;

--
-- Constraints for table `storagetype`
--
ALTER TABLE `storagetype`
  ADD CONSTRAINT `storagetype_ibfk_1` FOREIGN KEY (`ContractID`) REFERENCES `contract` (`Contract_ID`);

--
-- Constraints for table `supply`
--
ALTER TABLE `supply`
  ADD CONSTRAINT `fk_BrandPartner` FOREIGN KEY (`BrandPartner_ID`) REFERENCES `brandpartner` (`BrandPartner_ID`),
  ADD CONSTRAINT `supply_ibfk_1` FOREIGN KEY (`employee_id`) REFERENCES `employee` (`Employee_ID`);

--
-- Constraints for table `supply_details`
--
ALTER TABLE `supply_details`
  ADD CONSTRAINT `fk_barcode` FOREIGN KEY (`product_barcode`) REFERENCES `product` (`product_barcode`),
  ADD CONSTRAINT `fk_supply` FOREIGN KEY (`supply_id`) REFERENCES `supply` (`supply_id`);

--
-- Constraints for table `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `fk_user_employee` FOREIGN KEY (`employee_id`) REFERENCES `employee` (`Employee_ID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
