# NewsCrawling

<img src="https://github.com/EomTaeWook/NewsCrawling/blob/main/Image/CrawlingResult.png" width="100%"></img>
<img src="https://github.com/EomTaeWook/NewsCrawling/blob/main/Image/CrawlingResult.png" width="100%"></img>

다운로드 : https://github.com/EomTaeWook/NewsCrawling/releases/tag/1.0.0

개발환경

    C# .net core 3.1

실행 방법

    config.json

        keywords : 뉴스 내용이나 타이틀에서 키워드가 포함되어있는 것들만 추출

        RequestDelayMilliseconds : 다음 요청까지 대기 시간(자주 요청하는 경우 차단을 막기 위함)

    mailConfig.json

        SmtpHost : smtp 서버 주소

        SmtpPort : smtp 포트

        SmtpSender : 보내는 사람

        SmtpReceiver : 받는 사람(여러명)

        MailTitle : 메일 제목

        MailUserId : 메일 계정의 ID
        
        MailUserPassword : 메일 계정의 비밀번호

