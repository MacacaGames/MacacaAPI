//
//  UnityTapticVibrationPlugin.m
//  Vibration
//
//  Created by 黄建豪 on 2017/5/30.
//  Copyright © 2017年 黄建豪. All rights reserved.
//
#import <UIKit/UIKit.h>
#import <Foundation/NSURLConnection.h>
#import <Foundation/NSURL.h>
#import <AVFoundation/AVFoundation.h>
#import "UnityAppController.h"

extern bool _unityAppReady;

@interface CloudMacacaAppController : UnityAppController

@end
//加上這行可以複寫 Unity 原生的 AppController
IMPL_APP_CONTROLLER_SUBCLASS(CloudMacacaAppController)
@implementation CloudMacacaAppController
- (id)init
{
    
    NSLog(@"AVAudioSession",@"AVAudioSession");
    AVAudioSession *session =  [AVAudioSession sharedInstance];
    [session setCategory:AVAudioSessionCategoryAmbient withOptions:AVAudioSessionCategoryOptionMixWithOthers error:nil];
    return [super init];
}
- (void)application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forLocalNotification:(UILocalNotification *)notification completionHandler:(void (^)())completionHandler
{
    [super application:application handleActionWithIdentifier:identifier forLocalNotification:notification completionHandler:completionHandler];
    [self notifyUnityOfAction:identifier inNotification:notification completionHandler:completionHandler];
    
   
}

- (void)notifyUnityOfAction:(NSString*)identifier inNotification:(UILocalNotification*)notification completionHandler:(void (^)())completionHandler
{
    if (_unityAppReady)
    {
        NSArray *parts = [identifier componentsSeparatedByString:@":"];
        if (parts.count == 3) {
            NSString *gameObject = parts[0];
            NSString *handlerMethod = parts[1];
            NSString *action = parts[2];
            UnitySendMessage(strdup([gameObject UTF8String]), strdup([handlerMethod UTF8String]), strdup([action UTF8String]));
            UnityBatchPlayerLoop();
        }
        
        if (completionHandler != nil)
            completionHandler();
    }
    else
    {
        NSNotificationCenter * __weak center = [NSNotificationCenter defaultCenter];
        id __block token = [center addObserverForName:@"UnityReady" object:nil queue:[NSOperationQueue mainQueue] usingBlock:^(NSNotification *note) {
            _unityAppReady = true;
            [center removeObserver:token];
            [self notifyUnityOfAction:identifier inNotification:notification completionHandler:completionHandler];
        }];
    }
}
@end

