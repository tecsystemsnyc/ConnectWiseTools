laborData = read.csv("LaborData.csv")

#Remove Outliers
#laborData <- laborData[-c(358, 364, 368, 399, 405, 410, 430, 442, 443, 473, 474, 476, 481, 506),]
#laborData <- laborData[-c(430, 476, 506),]

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

#sub100LaborData <- with(laborData, laborData[(tot_mat + tot_sub) < 1e5,])
#over100LaborData <- with(laborData, laborData[(tot_mat + tot_sub) >= 1e5, ])

#subEngModel <- lm(En_hours ~ sum(tot_mat, tot_sub), data = sub100LaborData)
#overEngModel <- lm(En_hours ~ sum(tot_mat, tot_sub), data = over100LaborData)

#summary(subEngModel)
#summary(overEngModel)

#par(mfrow = c(1, 2))
#plot(predict(subEngModel), sub100LaborData$En_hours, xlab = "predicted", ylab = "actual", title="Under 100k")
#abline(a = 0, b=1)
#plot(predict(overEngModel), over100LaborData$En_hours, xlab = "predicted", ylab = "actual", title="Over 100k")
#abline(a = 0, b = 1)

#par(mfrow = c(2, 2))
#plot(subEngModel)
#par(mfrow = c(2, 2))
#plot(overEngModel)

