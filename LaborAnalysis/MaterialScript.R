laborData = read.csv("LaborData.csv")
#laborData$total_hours = laborData$Cm_hours + laborData$En_hours + laborData$Pm_hours + laborData$Gr_hours + laborData$Sf_hours
#laborData$total_cost = laborData$tot_mat + laborData$tot_sub
#laborData <- laborData[order(laborData$total_cost),]

#plot(laborData$total_hours, laborData$En_hours, col = "red")
#abline(lm(laborData$En_hours ~ laborData$total_hours), col = "red")
#points(laborData$total_hours, laborData$Pm_hours, col = "green")
#abline(lm(laborData$Pm_hours ~ laborData$total_hours), col = "green")
#points(laborData$total_hours, laborData$Gr_hours, col = "purple")
#abline(lm(laborData$Gr_hours~laborData$total_hours), col = "purple")
#points(laborData$total_hours, laborData$Sf_hours, col = "blue")
#abline(lm(laborData$Sf_hours~laborData$total_hours), col = "blue")
#points(laborData$total_hours, laborData$Cm_hours, col = "pink")
#abline(lm(laborData$Cm_hours~laborData$total_hours), col = "pink")


#Remove Outliers
#Below were determined based on three rounds of outlier analysis
#laborData <- laborData[-c(166,232,234,282,286,294,299,345,366,368,373,377,386,387,393,400,401,403,404,405,406,410,417,418,420,424,426,429,430,432,433,441,442,444,462,467,468,471,472,473,474,476,477,480,482,486,492,494,499,506),]

#Prepare Data
#laborData$above100k <- ifelse(laborData$tot_mat + laborData$tot_sub > 100000, 1, 0)
laborData <- laborData[which(laborData$tot_mat > 0 & laborData$tot_sub > 0),]
laborData <- laborData[which(laborData$En_hours > 0),]
laborData <- laborData[which(laborData$Pm_hours > 0),]
laborData <- laborData[which(laborData$Sf_hours > 0),]
laborData <- laborData[which(laborData$Gr_hours > 0),]
laborData <- laborData[which(laborData$Cm_hours > 0),]

above100kData <- laborData[which(laborData$tot_mat + laborData$tot_sub >= 100000),]
below100kData <- laborData[which(laborData$tot_mat + laborData$tot_sub < 100000),]

#Create Models
aboveEngModel <- lm(En_hours ~ tot_mat + tot_sub, data = above100kData)
belowEngModel <- lm(En_hours ~ tot_mat + tot_sub, data = below100kData)

abovePmModel <- lm(Pm_hours ~ tot_mat + tot_sub, data = above100kData)
belowPmModel <- lm(Pm_hours ~ tot_mat + tot_sub, data = below100kData)

aboveSfModel <- lm(Sf_hours ~ tot_mat + tot_sub, data = above100kData)
belowSfModel <- lm(Sf_hours ~ tot_mat + tot_sub, data = below100kData)

aboveGrModel <- lm(Gr_hours ~ tot_mat + tot_sub, data = above100kData)
belowGrModel <- lm(Gr_hours ~ tot_mat + tot_sub, data = below100kData)

aboveCmModel <- lm(Cm_hours ~ tot_mat + tot_sub, data = above100kData)
belowCmModel <- lm(Cm_hours ~ tot_mat + tot_sub, data = below100kData)

#Display Models
#Engineering
summary(aboveEngModel)
plot(predict(aboveEngModel), above100kData$En_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Engineering Above 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(aboveEngModel)
title("Engineering Above 100k", outer = TRUE, line = -2)

summary(belowEngModel)
plot(predict(belowEngModel), below100kData$En_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Engineering Below 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(belowEngModel)
title("Engineering Below 100k", outer = TRUE, line = -2)

#Project Management
summary(abovePmModel)
plot(predict(abovePmModel), above100kData$Pm_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Project Management Above 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(abovePmModel)
title("Project Management Above 100k", outer = TRUE, line = -2)

summary(belowPmModel)
plot(predict(belowPmModel), below100kData$Pm_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Project Management Below 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(belowPmModel)
title("Project Management Below 100k", outer = TRUE, line = -2)

#Software
summary(aboveSfModel)
plot(predict(aboveSfModel), above100kData$Sf_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Software Above 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(aboveSfModel)
title("Software Above 100k", outer = TRUE, line = -2)

summary(belowSfModel)
plot(predict(belowSfModel), below100kData$Sf_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Software Below 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(belowSfModel)
title("Software Below 100k", outer = TRUE, line = -2)

#Graphics
summary(aboveGrModel)
plot(predict(aboveGrModel), above100kData$Gr_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Graphics Above 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(aboveGrModel)
title("Graphics Above 100k", outer = TRUE, line = -2)

summary(belowGrModel)
plot(predict(belowGrModel), below100kData$Gr_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Graphics Below 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(belowGrModel)
title("Graphics Below 100k", outer = TRUE, line = -2)

#Commissioning
summary(aboveCmModel)
plot(predict(aboveCmModel), above100kData$Cm_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Commisioning Above 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(aboveCmModel)
title("Commisioning Above 100k", outer = TRUE, line = -2)

summary(belowCmModel)
plot(predict(belowCmModel), below100kData$Cm_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)
title("Commissioning Below 100k", outer = TRUE, line = -2)


par(mfrow = c(2, 2))
plot(belowCmModel)
title("Commisioning Below 100k", outer = TRUE, line = -2)


