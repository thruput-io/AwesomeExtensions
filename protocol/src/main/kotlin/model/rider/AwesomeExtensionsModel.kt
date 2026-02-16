package model.rider

import com.jetbrains.rd.generator.nova.*
import com.jetbrains.rd.generator.nova.csharp.CSharp50Generator
import com.jetbrains.rd.generator.nova.kotlin.Kotlin11Generator
import java.io.File

object AwesomeExtensionsRoot : Root()

object AwesomeExtensionsModel : Ext(AwesomeExtensionsRoot) {
    init {
        // No specific properties needed for now, just establishing the root
    }
}
