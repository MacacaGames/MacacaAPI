//
//  CMShareController.m
//  CloudMacacaIOSNative
//
//  Created by 黃建豪 on 2017/12/14.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import "CMShareController.h"
#import "MacacaUtils.h"
#import <UIKit/UIKit.h>
#import "GifItemProvider.h"
@implementation CMShareController

+(void) ShareTextForTitle:(NSString*) subject andForMsg:(NSString*)message{
    
    NSArray *items;
    items = @[message];
    UIActivityViewController *activity = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:Nil];

    if(subject != nil && subject.length > 0)[activity setTitle:subject];
    
    UIViewController *rootViewController = [MacacaUtils GetKeyWindow].rootViewController;
    //if iPhone
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone) {
        [rootViewController presentViewController:activity animated:YES completion:nil];
    }
    //if iPad
    else {
        // Change Rect to position Popover
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activity];
        [popup presentPopoverFromRect:CGRectMake(rootViewController.view.frame.size.width/2, rootViewController.view.frame.size.height/4, 0, 0)inView:rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
}

+(void) ShareImageForPath:(NSString*) path andForTitle:(NSString*) subject andForMsg:(NSString*) message{
    UIImage *image = [UIImage imageWithContentsOfFile:path];
    NSArray *items;
    if (message.length > 0 && message != nil)
        items = @[message, image];
    else
        items = @[image];

    
    UIActivityViewController *activity = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:Nil];
   
    if(subject != nil && subject.length > 0)[activity setValue:subject forKey:@"subject"];
    
    UIViewController *rootViewController = [MacacaUtils GetKeyWindow].rootViewController;
    //if iPhone
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone) {
        [rootViewController presentViewController:activity animated:YES completion:Nil];
    }
    //if iPad
    else {
        // Change Rect to position Popover
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activity];
        [popup presentPopoverFromRect:CGRectMake(rootViewController.view.frame.size.width/2, rootViewController.view.frame.size.height/4, 0, 0)inView:rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
}
+(void)ShareGifForPath:(NSString*) path andForTitle:(NSString*) subject andForMsg:(NSString*) message{
    NSArray *items;
   
    GifItemProvider *gifItem = [[GifItemProvider alloc] initWithPlaceholderItem:path];
    
    
    if (message.length > 0 && message != nil)
        items = @[gifItem,message];
    else
        items = @[gifItem];
    
    
    UIActivityViewController *activity = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:Nil];
    if(subject != nil && subject.length > 0)
        [activity setValue:subject forKey:@"subject"];
    
    UIViewController *rootViewController = [MacacaUtils GetKeyWindow].rootViewController;
    //if iPhone
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone) {
        [rootViewController presentViewController:activity animated:YES completion:Nil];
    }
    //if iPad
    else {
        // Change Rect to position Popover
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activity];
        [popup presentPopoverFromRect:CGRectMake(rootViewController.view.frame.size.width/2, rootViewController.view.frame.size.height/4, 0, 0)inView:rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
}


+(void)ShareVideoForPath:(NSString*) path andForTitle:(NSString*) subject andForMsg:(NSString*) message{
    NSURL *videoURL = [NSURL fileURLWithPath:path];
    NSArray *items;
    
    if (message.length > 0 && message != nil)
         items = @[message, videoURL];
    else
         items = @[videoURL];

    
    UIActivityViewController *activity = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:Nil];
    if(subject != nil && subject.length > 0)
        [activity setValue:subject forKey:@"subject"];
    
    UIViewController *rootViewController = [MacacaUtils GetKeyWindow].rootViewController;
    //if iPhone
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone) {
        [rootViewController presentViewController:activity animated:YES completion:Nil];
    }
    //if iPad
    else {
        // Change Rect to position Popover
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activity];
        [popup presentPopoverFromRect:CGRectMake(rootViewController.view.frame.size.width/2, rootViewController.view.frame.size.height/4, 0, 0)inView:rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
}

/*

+(void) CM_ShowWithText:(NSString*)text withFilePaths:(NSString*)filePaths withSubject:(NSString*)subject{
    return [[CMShareController sharedInstance] initWithText:text withFilePaths:filePaths withSubject:subject];
}

-(void) initWithText:(NSString*)text withFilePaths:(NSString*)filePaths withSubject:(NSString*)subject{

    NSString *mText = filePaths;
    
    NSString *mSubject = subject;
    
    NSString *mfilePath = filePaths;
    
    
    NSMutableArray *items = [NSMutableArray new];
    
    if(mText != NULL && mText.length > 0){
        
        [items addObject:mText];
        
    }
    
    if(mfilePath != NULL && mfilePath.length > 0){
        NSArray *paths = [mfilePath componentsSeparatedByString:@";"];
        int i;
        for (i = 0; i < [paths count]; i++) {
            NSString *path = [paths objectAtIndex:i];
            
            if([path hasPrefix:@"http"])
            {
                NSURL *url = [NSURL URLWithString:path];
                NSError *error = nil;
                NSData *dataImage = [NSData dataWithContentsOfURL:url options:0 error:&error];
                
                if (!error) {
                    UIImage *imageFromUrl = [UIImage imageWithData:dataImage];
                    [items addObject:imageFromUrl];
                } else {
                    [items addObject:url];
                }
            }
            else if ( [self isStringValideBase64:path]){
                NSData* imageBase64Data = [[NSData alloc]initWithBase64EncodedString:path options:0];
                UIImage* image = [UIImage imageWithData:imageBase64Data];
                if (image != nil){
                    [items addObject:image];
                }
                else{
                    NSURL *formattedURL = [NSURL fileURLWithPath:path];
                    [items addObject:formattedURL];
                }
            }
            else{
                NSFileManager *fileMgr = [NSFileManager defaultManager];
                if([fileMgr fileExistsAtPath:path]){
                    NSData *dataImage = [NSData dataWithContentsOfFile:path];
                    UIImage *imageFromUrl = [UIImage imageWithData:dataImage];
                    if (imageFromUrl != nil){
                        [items addObject:imageFromUrl];
                    }else{
                        NSURL *formattedURL = [NSURL fileURLWithPath:path];
                        [items addObject:formattedURL];
                    }
                }else{
                    //ShowAlertMessage(@"Error", [NSString stringWithFormat:@"Cannot find file %@", path]);
                }
            }
        }
    }
    
    UIActivityViewController *activity = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:Nil];
    
    if(mSubject != NULL) {
        [activity setValue:mSubject forKey:@"subject"];
    } else {
        [activity setValue:@"" forKey:@"subject"];
    }
    
    UIViewController *rootViewController = [Utils GetKeyWindow].rootViewController;
    //if iPhone
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone) {
        [rootViewController presentViewController:activity animated:YES completion:Nil];
    }
    //if iPad
    else {
        // Change Rect to position Popover
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activity];
        [popup presentPopoverFromRect:CGRectMake(rootViewController.view.frame.size.width/2, rootViewController.view.frame.size.height/4, 0, 0)inView:rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
}

-(BOOL) isStringValideBase64:(NSString*)string{
    
    NSString *regExPattern = @"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$";
    
    NSRegularExpression *regEx = [[NSRegularExpression alloc] initWithPattern:regExPattern options:NSRegularExpressionCaseInsensitive error:nil];
    NSUInteger regExMatches = [regEx numberOfMatchesInString:string options:0 range:NSMakeRange(0, [string length])];
    return regExMatches != 0;
}

CMShareController* instance;
+ (CMShareController *)sharedInstance{
    static CMShareController *sharedInstance = nil;
    static dispatch_once_t onceToken;
    
    dispatch_once(&onceToken, ^{
        sharedInstance = [[CMShareController alloc] init];
        // Do any other initialisation stuff here
    });
    return sharedInstance;
}
*/

@end
