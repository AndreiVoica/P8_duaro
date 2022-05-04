# Tuning

## PPO hyperparameters

### Gamma:

(default = 0.99) Discount factor for future rewards coming from the environment. This can be thought of as how far into the future the agent should care about possible rewards. In situations when the agent should be acting in the present to prepare for rewards in the distant future, this value should be large. In cases when rewards are more immediate, it can be smaller. Must be strictly smaller than 1. Typical range: 0.8 - 0.995

### Lambd:

Default = 0.95 Regularization parameters (lambda) used when calculating the Generalized Advantage Estimate (GAE). This can be thought of as how much the agent relies on its current estimate when calculating an updated value estimate. Low values correspond to relying more on the current value estimate (which can be high bias), and high values corresponding to relying more on the actual rewards received in the environment (which can be high variance). The parameter provides a trade-off between the two, and the right value can lead to a more stable training process.

Type range: 0.9 – 0.95

### Buffer_size:

Default = 10240 for PPO and 50000 for SAC

PPO: Number of experiences to collect before updating the policy model. Corresponds to how many experiences before we do any learning or updating of the model. This should be multiple times larger than batch_size. Typically, larger buffer_size corresponds to more stable training updates.

Typical range: 2048 - 409600

Num_epoch:

(default = 3) Number of passes to make through the experience buffer when performing gradient descent optimization. The larger the batch_size, the larger it is acceptable to make this. Decreasing this will ensure more stable updates, at the cost of slower learning.

Typical range: 3 – 10

Batch_size:

Number of experiences in each iteration of gradient descent. This should always be multiple times smaller than buffer_size. If you are using continuous actions, this value should be large (on the order of 1000s). If you are using only discrete actions, this value should be smaller (on the order of 10s). Typical range: (Continuous - PPO): 512 - 5120; (Continuous - SAC): 128 - 1024; (Discrete, PPO & SAC): 32 - 512.

Learning_rate:

(default = 3e-4) Initial learning rate for gradient descent. Corresponds to the strength of each gradient descent update step. This should typically be decreased if training is unstable, and the reward does not consistently increase. Typical range: 1e-5 - 1e-3

Learning_rate_schedule:

(default = linear for PPO and constant for SAC) Determines how learning rate changes over time. For PPO, we recommend decaying learning rate until max_steps so learning converges more stably. However, for some cases (e.g. training for an unknown amount of time) this feature can be disabled. For SAC, we recommend holding learning rate constant so that the agent can continue to learn until its Q function converges naturally. linear decays the learning_rate linearly, reaching 0 at max_steps, while constant keeps the learning rate constant for the entire training run.

Beta:

(default = 5.0e-3) Strength of the entropy regularization, which makes the policy "more random." This ensures that agents properly explore the action space during training. Increasing this will ensure more random actions are taken. This should be adjusted such that the entropy (measurable from TensorBoard) slowly decreases alongside increases in reward. If entropy drops too quickly, increase beta. If entropy drops too slowly, decrease beta. Typical range: 1e-4 - 1e-2

Epsilon:

(default = 0.2) Influences how rapidly the policy can evolve during training. Corresponds to the acceptable threshold of divergence between the old and new policies during gradient descent updating. Setting this value small will result in more stable updates but will also slow the training process. Typical range: 0.1 - 0.3

Network settings

Normalize:

(default = false) Whether normalization is applied to the vector observation inputs. This normalization is based on the running average and variance of the vector observation. Normalization can be helpful in cases with complex continuous control problems but may be harmful with simpler discrete control problems.