package com.dwin.du.config;

import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.ApplicationListener;
import org.springframework.stereotype.Component;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

@Component
public class SwaggerUiLauncher implements ApplicationListener<ApplicationReadyEvent> {

    private static final Logger logger = LoggerFactory.getLogger(SwaggerUiLauncher.class);

    @Override
    public void onApplicationEvent(ApplicationReadyEvent event) {
        logger.info("Swagger UI is available at: http://localhost:8080/swagger-ui/index.html");
    }
}