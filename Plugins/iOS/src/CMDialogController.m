//
//  MikiDialogController.m
//  MikiAPI_iOS_Framework
//
//  Created by 黄建豪 on 2017/9/2.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import "CMDialogController.h"
#import "MacacaUtils.h"
#import <StoreKit/SKStoreReviewController.h>
#import <StoreKit/StoreKit.h>
#import "LSLDatePickerDialog.h"
#import <MessageUI/MessageUI.h>
#import <MessageUI/MFMailComposeViewController.h>
AlertDialogCallback alertDialogCallback;
DatePickerDialogCallback datePickerDialogCallback;


@implementation CMDialogController


-(void) SendEmail:(NSString*)to Subject:(NSString*)subject Body:(NSString *)body
{
    //if (![MFMailComposeViewController canSendMail]) return;
    
    MFMailComposeViewController *mailViewController = [[MFMailComposeViewController alloc] init];
    mailViewController.mailComposeDelegate = self;
    
    // subject
    [mailViewController setSubject:subject];
    
    // mail to
    NSArray *toRecipients = [NSArray arrayWithObject:to];
    [mailViewController setToRecipients:toRecipients];
    
    // body
    [mailViewController setMessageBody:body isHTML:NO];
    
    [[MacacaUtils GetKeyWindow].rootViewController presentViewController: mailViewController animated:YES completion:NULL];
}

- (void)mailComposeController:(MFMailComposeViewController *)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError *)error {
 
     [[MacacaUtils GetKeyWindow].rootViewController dismissViewControllerAnimated:YES completion:NULL];
}


+(void) ShowRateUsDialog{
    [SKStoreReviewController requestReview];
}

+(void) ShowAlertDialogForTitle:(NSString*)title andForMsg:(NSString*)msg andForOkText:(NSString*)okText {
    UIAlertController * alert = [UIAlertController
                                 alertControllerWithTitle:title
                                 message:msg
                                 preferredStyle:UIAlertControllerStyleAlert];
    
    
    
    UIAlertAction* yesButton = [UIAlertAction
                                actionWithTitle:okText
                                style:UIAlertActionStyleDefault
                                handler:^(UIAlertAction * action) {
                                    //Handle your yes please button action here
                                    alertDialogCallback(0);
                                }];
    
    UIAlertAction* noButton = [UIAlertAction
                               actionWithTitle:@"No, thanks"
                               style:UIAlertActionStyleDefault
                               handler:^(UIAlertAction * action) {
                                   //Handle no, thanks button
                               }];
    
    [alert addAction:yesButton];
    //[alert addAction:noButton];
    
    [[MacacaUtils GetKeyWindow].rootViewController presentViewController:alert animated:YES completion:nil];
}
+(void) ShowToastMessageForMsg:(NSString*)msg{

    UIAlertController *alert = [UIAlertController alertControllerWithTitle:nil
                                                   message:msg
                                                            preferredStyle:UIAlertControllerStyleActionSheet];

    [[MacacaUtils GetKeyWindow].rootViewController presentViewController:alert animated:YES completion:nil];

    int duration = 1; // duration in seconds

    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, duration * NSEC_PER_SEC), dispatch_get_main_queue(), ^{
        [alert dismissViewControllerAnimated:YES completion:nil];
    });
}


+(void) SetDatePickerDialogCallback:(DatePickerDialogCallback)cb{
    datePickerDialogCallback = cb;
}

+(void) SetAlertDialogCallback :(AlertDialogCallback) cb{
    alertDialogCallback = cb;
}

+(void) ShowDatePickerWithCallback:(NSString*)okText andForCancel:(NSString*)cancelText{
    LSLDatePickerDialog *dpDialog = [[LSLDatePickerDialog alloc] init];

    [dpDialog showWithTitle:@"DatePicker" doneButtonTitle:okText cancelButtonTitle:cancelText
                defaultDate:[NSDate date] minimumDate:nil maximumDate:nil datePickerMode:UIDatePickerModeDate
                   callback:^(NSDate * _Nullable date){
                       if(date)
                       {
                           NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
                           [formatter setDateFormat:@"YYYY/MM/dd"];
                           datePickerDialogCallback([[formatter stringFromDate:date] UTF8String]);
                       }
                   }
     ];
}

+(void) ShowAlertDialogForTitleWithCallback:(NSString*)title andForMsg:(NSString*)msg andForPositiveText:(NSString*)positiveText andForNegitiveText: (NSString*)negativeText{
    
    NSLog(@"In CMDialogController ShowAlertDialogForTitleWithCallback");

    UIAlertController * alert = [UIAlertController
        alertControllerWithTitle:title
        message:msg
        preferredStyle:UIAlertControllerStyleAlert];
    
    UIAlertAction* yesButton = [UIAlertAction
                                actionWithTitle:positiveText
                                style:UIAlertActionStyleDefault
                                handler:^(UIAlertAction * action) {
                                    //Handle your yes please button action here
                                    alertDialogCallback(0);
                                    
                                }];
    
    UIAlertAction* noButton = [UIAlertAction
                               actionWithTitle:negativeText
                               style:UIAlertActionStyleDefault
                               handler:^(UIAlertAction * action) {
                                   //Handle no, thanks button
                                   alertDialogCallback(1);
                               }];
    [alert addAction:yesButton];
    [alert addAction:noButton];

    [[MacacaUtils GetKeyWindow].rootViewController presentViewController:alert animated:YES completion:nil];
}
@end
