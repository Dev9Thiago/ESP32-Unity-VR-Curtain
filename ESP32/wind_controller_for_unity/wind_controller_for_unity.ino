const int POT_X_PIN = 32;
const int POT_Z_PIN = 33;

const int NUM_SAMPLES = 5;

int samplesX[NUM_SAMPLES] = {0, 0, 0, 0, 0};
int samplesZ[NUM_SAMPLES] = {0, 0, 0, 0, 0};

int sampleIndex = 0;

float mapFloat(float x,
               float inMin,
               float inMax,
               float outMin,
               float outMax)
{
  return (x - inMin) *
         (outMax - outMin) /
         (inMax - inMin) +
         outMin;
}

void setup()
{
  Serial.begin(115200);

  analogReadResolution(12);

  pinMode(POT_X_PIN, INPUT);
  pinMode(POT_Z_PIN, INPUT);

  delay(1000);
}

void loop()
{
  samplesX[sampleIndex] = analogRead(POT_X_PIN);
  samplesZ[sampleIndex] = analogRead(POT_Z_PIN);

  sampleIndex = (sampleIndex + 1) % NUM_SAMPLES;

  float sumX = 0;
  float sumZ = 0;

  for (int i = 0; i < NUM_SAMPLES; i++)
  {
    sumX += samplesX[i];
    sumZ += samplesZ[i];
  }

  float averageX = sumX / NUM_SAMPLES;
  float averageZ = sumZ / NUM_SAMPLES;


  // ----------------------------------------------------------
  // 3. Convert ADC readings into wind values
  //
  // ADC:       0 ---- 4095
  // Wind:    -20 ---- +20
  // ----------------------------------------------------------

  float windX = mapFloat(
    averageX,
    0.0,
    4095.0,
    -20.0,
    20.0
  );

  float windZ = mapFloat(
    averageZ,
    0.0,
    4095.0,
    -20.0,
    20.0
  );


  // ----------------------------------------------------------
  // 4. Send CSV data to Unity
  //
  // Example:
  // -12.43,8.71
  // ----------------------------------------------------------

  Serial.print(windX, 2);
  Serial.print(",");
  Serial.println(windZ, 2);


  // ----------------------------------------------------------
  // 5. Update at approximately 20 Hz
  // ----------------------------------------------------------

  delay(50);
}