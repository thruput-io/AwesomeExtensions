import org.jetbrains.kotlin.gradle.tasks.KotlinCompile

plugins {
    alias(libs.plugins.kotlinJvm)
    id("com.jetbrains.rdgen") version libs.versions.rdGen.get()
}

repositories {
    maven { setUrl("https://cache-redirector.jetbrains.com/maven-central") }
}

configure<com.jetbrains.rd.generator.gradle.RdGenExtension> {
    val modelDir = File(projectDir, "src/main/kotlin/model")
    val csOutput = File(rootDir, "src/dotnet/AwesomeExtensions/Rider")
    val ktOutput = File(rootDir, "src/rider/main/kotlin/com/jetbrains/rider/plugins/awesomeextensions/model")

    verbose = true
    hashFolder = "build/rdgen"

    generator {
        language = "csharp"
        transform = "ext"
        root = "model.rider.AwesomeExtensionsRoot"
        namespace = "AwesomeExtensions.Rider"
        directory = csOutput.canonicalPath
    }

    generator {
        language = "kotlin"
        transform = "ext"
        root = "model.rider.AwesomeExtensionsRoot"
        namespace = "com.jetbrains.rider.plugins.awesomeextensions.model"
        directory = ktOutput.canonicalPath
    }
}

tasks.withType<KotlinCompile> {
    kotlinOptions.jvmTarget = "17"
}

dependencies {
    implementation(libs.rdGen)
}
