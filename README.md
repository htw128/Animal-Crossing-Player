# Animal Crossing New Horizen Music Player

## 说明

根据你的IP地址，使用`api.weatherapi.com`来获取天气信息。如果打开以后获取到的天气信息不正确，请把这个API地址添加到代理的豁免列表里。

为了避免版权问题，本开源库不包含Audiokinetic以及Nintendo的资产。如果你需要完整使用这个项目，你需要使用Audiokinetic Launcher将Wwise Integration集成到项目中，并将对应的音乐wav文件存放至Wwise工程的对应位置。

## TODO

-  [x] 不同天气切换淡入淡出
-  [x] 音量滑块
-  [x] 跟随当前天气情况播放对应音乐
-  [x] 刷新按钮防爆破
-  [ ] 整点切换效果优化
-  [ ] 刷新过程中显示占位符
-  [ ] 获取信息失败处理
-  [ ] 发布前撰写使用指南
-  [ ] 昼夜切换
-  [ ] 手动选择时间和天气
-  [ ] 自定义岛屿主题曲
-  [ ] 节日播放对应音乐（新年除外）
-  [ ] 新年倒计时相关音乐
-  [ ] 各处地点音乐（博物馆除外）
-  [ ] 博物馆对应音乐
-  [ ] 缓存命中+IP地址判断

## 发布前Checklist

移除天气API Key

移除soundbank

### 开发笔记
[这个](https://www.ip2location.io/)API好用，但是不知道为啥注册不上。先码了。

和风天气和心知天气的数据都很好。但是和风要JWT，心知也要搭服务器做私钥签名……
