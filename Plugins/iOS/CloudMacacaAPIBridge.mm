//
//  UnityTapticVibrationPlugin.m
//  Vibration
//
//  Created by 黄建豪 on 2017/5/30.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import <CloudMacacaIOSNative/CloudMacacaAPIController.h>
#import <AudioToolbox/AudioToolbox.h>
#import <UIKit/UIKit.h>

extern "C"{
    //Vibration
    void doCMVibrationAsPop();
    void doCMVibrationAsPeek();
    void doCMVibrationAsNope();
    
    //Dialog
    void doCMShowToastMessage(char* msg);
    void doCMShowAlertDialog(char* title,char* msg,char* okText);
    void doCMShowAlertDialogWithCallback(char* title,char* msg,char* positiveBtnText,char* negativeBtnText);
    
    void doCMShowDatePickerWithCallback(char* okText, char* cancelText);
    void doSetDatePickerCallback(DatePickerDialogCallback cb);
    void doSetAlertDialogCallback(AlertDialogCallback cb);
    
    //Rate us
    void doCMShowRateUsDialog();
    
    //Share
    void doCMShowShare(char* filePath,char* subject,char* msg, int typeId);
    
    //void doCMShowShare(char* msg,char* subject,char* filePath);

    //GameCenter
    void doShowAchievement();

    void doShowLeaderboard();

    void doUnlockAchievent(char* achievementId);
 
    void doUploadLeaderboard(char* boardId,int score);

    void doGameCenterInit();

    void doRequestIDFA();
    void doSetRequestIDFACallback(RequestCallback cb);
    
}

//Vibration
void doCMVibrationAsPop(){
    [CloudMacacaAPIController CM_VibrationAsPop];
}
void doCMVibrationAsPeek(){
    [CloudMacacaAPIController CM_VibrationAsPeek];
}
void doCMVibrationAsNope(){
    [CloudMacacaAPIController CM_VibrationAsNope];
}

//Dialog
void doCMShowToastMessage(char* msg){
    [CloudMacacaAPIController CM_ShowToastMessage:[NSString stringWithUTF8String:msg]];
}
void doCMShowAlertDialog(char* title,char* msg,char* okText){
    NSString* nsTitle =[NSString stringWithUTF8String:title];
    NSString* nsMsg =[NSString stringWithUTF8String:msg];
    NSString* nsOk =[NSString stringWithUTF8String:okText];
    [CloudMacacaAPIController CM_ShowAlertDialogForTitle:nsTitle andForMsg:nsMsg andForOkText:nsOk];
}
//Rate us
void doCMShowRateUsDialog(char* msg,char* canelTest,char* rateText,char*laterText,char* AppleStoreId){
    NSString* nsMsg =[NSString stringWithUTF8String:msg];
    NSString* nsCancel =[NSString stringWithUTF8String:canelTest];
    NSString* nsRate =[NSString stringWithUTF8String:rateText];
    NSString* nsLater =[NSString stringWithUTF8String:laterText];
    NSString* nsAppleStoreId =[NSString stringWithUTF8String:AppleStoreId];
    [CloudMacacaAPIController CM_ShowRateUsDialog:nsMsg andForCancelText:nsCancel  andForRateText:nsRate  andForLaterText:nsLater andForAppleID:nsAppleStoreId];
}


void doCMShowAlertDialogWithCallback(char* title,char* msg,char* positiveBtnText,char* negativeBtnText){
    
    NSLog(@"In doCMShowAlertDialogWithCallback");
    NSString* nsMsg =[NSString stringWithUTF8String:msg];
    NSString* nsTitle =[NSString stringWithUTF8String:title];
    NSString* nsPositiveBtnText =[NSString stringWithUTF8String:positiveBtnText];
    NSString* nsNegativeBtnText =[NSString stringWithUTF8String:negativeBtnText];
    [CloudMacacaAPIController CM_ShowAlertDialogWithCallbackForTitle:nsTitle andForMsg:nsMsg andForPositiveText:nsPositiveBtnText andForNegitiveText:nsNegativeBtnText];
}
void doSetAlertDialogCallback(AlertDialogCallback cb){
    NSLog(@"In doSetAlertDialogCallback");

    [CloudMacacaAPIController CM_SetAlertDialogCallback:cb];
}

void doCMShowDatePickerWithCallback(char* okText, char* cancelText){
    NSString* nsOkText =[NSString stringWithUTF8String:okText];
    NSString* nsCancelText =[NSString stringWithUTF8String:cancelText];
    [CloudMacacaAPIController CM_ShowDatePickerWithCallback:nsOkText andForCancel:nsCancelText];
}
void doSetDatePickerCallback(DatePickerDialogCallback cb){
    [CloudMacacaAPIController CM_SetDatePickerDialogCallback:cb];
}

//Show Share
void doCMShowShare(char* filePath,char* subject,char* msg,int typeId){
    NSString* nsMsg =[NSString stringWithUTF8String:msg];
    NSString* nsSubject =[NSString stringWithUTF8String:subject];
    NSString* nsFilePath =[NSString stringWithUTF8String:filePath];
    [CloudMacacaAPIController CM_ShowShareForFilePaths:nsFilePath andForSubject:nsSubject andForMsg:nsMsg andForTypeId:typeId];
}

//GameCenter
void doShowAchievement(){
    [CloudMacacaAPIController CM_ShowAchievement];
}

void doShowLeaderboard(){
    [CloudMacacaAPIController CM_ShowLeaderboard];
}

void doUnlockAchievent(char* achievementId){
    NSString* nsAchievementId =[NSString stringWithUTF8String:achievementId];
    [CloudMacacaAPIController CM_UnlockAnAchievement:nsAchievementId];
}
 
void doUploadLeaderboard(char* boardId,int score){
    NSString* nsBoardId =[NSString stringWithUTF8String:boardId];
    [CloudMacacaAPIController CM_UploadNewScoreForboardId:nsBoardId andForScore:score];
}

void doGameCenterInit(){
    [CloudMacacaAPIController CM_InitGameCenter];
}
void doRequestIDFA(){
    [CloudMacacaAPIController CM_RequestIDFA];
}

void doSetRequestIDFACallback(RequestCallback cb){
    [CloudMacacaAPIController CM_SetRequestIDFACallback:cb];
}