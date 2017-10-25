laborData = read.csv("LaborData.csv")

#Remove Outliers
#laborData <- laborData[-c(358, 364, 368, 399, 405, 410, 430, 442, 443, 473, 474, 476, 481, 506),]

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

#Display Models
summary(aboveEngModel)
plot(predict(aboveEngModel), above100kData$En_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)

par(mfrow = c(2, 2))
plot(aboveEngModel)

summary(belowEngModel)
plot(predict(belowEngModel), below100kData$En_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)

par(mfrow = c(2, 2))
plot(belowEngModel)



